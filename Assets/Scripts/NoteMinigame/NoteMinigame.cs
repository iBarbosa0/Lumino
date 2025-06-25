using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NoteMinigame : MonoBehaviour
{
    public Text messageText;         // Mensagem de feedback ("Acertou!" etc.)
    public Text levelText;           // Texto para mostrar o nível atual
    public Text musicVolumePercentageText; // Texto que mostra o volume em %

    public Image progressBarFill; // imagem da barra preenchida

    public Button startButton;       // Botão que inicia o minigame
    public Button backButton;        // Botão que volta ao menu principal

    public Transform leftNoteSlot;
    public Transform rightNoteSlot;
    public Transform coin1Slot;
    public Transform coin2Slot;
    public Transform[] feedbackCheckSlots; // Array com 3 posições (ex: 3 RectTransforms)

    public GameObject coin1Prefab;
    public GameObject coin2Prefab;
    public GameObject note5Prefab;
    public GameObject note10Prefab;
    public GameObject note20Prefab;
    public GameObject feedbackCheckPrefab; // Prefab do check

    public GameObject feedbackPanel;
    public GameObject mainMenuPanel; // Painel do menu principal
    public GameObject settingsPanel; // Painel de configurações

    public Slider musicVolumeSlider; // Slider do volume de música

    public Sprite[] requestedSprites; // Ordem: [1, 2, 5, 10, 20]

    public Animator correctAnimator;

    public int requestedAmount = 20; // Valor pedido pela senhora da caixa (nível 1 = 20)
    private bool interactionLocked = false;

    private int currentLevel = 1;
    private int lastRequestedAmount = -1;

    public AudioClip correctSound;    // Som de acerto
    public AudioClip wrongSound;      // Som de erro
    public AudioClip backgroundMusic; // Música de fundo
    private AudioSource audioSource;  // Componente AudioSource

    public List<int> availableDenominations = new List<int>();
    public List<Image> requestedAmountImages; // Lista com 3 imagens para mostrar os pedidos
    private List<int> requestedValues = new List<int>(); // Valores pedidos
    private List<GameObject> currentMoneyItems = new List<GameObject>();
    private List<GameObject> currentFeedbackChecks = new List<GameObject>(); // Lista de checks ativos
    private List<int> receivedValues = new List<int>();

    private int[] possibleRequestedValues = new int[] { 1, 2, 5, 10, 20 };

    private const string MusicVolumeKey = "MusicVolume";

    private Dictionary<int, Transform> notePositions = new Dictionary<int, Transform>();

    private void Start()
    {
        // Configuração AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.loop = true;      // Faz a música tocar em loop
        audioSource.playOnAwake = false;

        audioSource.Play();

        // Carrega volume salvo e aplica
        float savedVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 0.5f);
        musicVolumeSlider.value = savedVolume;
        audioSource.volume = savedVolume;

        if (musicVolumePercentageText != null)
        {
            int percent = Mathf.RoundToInt(savedVolume * 100f);
            musicVolumePercentageText.text = Mathf.RoundToInt((musicVolumeSlider.value / musicVolumeSlider.maxValue) * 100) + "%";
        }

        if (correctAnimator != null)
        {
            correctAnimator.Rebind();
            correctAnimator.Update(0f);
            correctAnimator.Play("Idle", 0, 0f);
        }

        // Conecta evento do slider
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);

        mainMenuPanel.SetActive(true); // Mostra o menu
    }

    public void StartMinigame()
    {
        mainMenuPanel.SetActive(false); // Esconde o menu
        StartNewRound();               // Inicia o jogo
    }

    private void StartNewRound()
    {
        notePositions.Clear();
        HideAllFeedbackChecks(); // Garante que nenhum check fique visível
        UpdateAvailableDenominations(currentLevel);
        GetRequestedValueMatchingOnly();
        interactionLocked = false;

        progressBarFill.fillAmount = 0f;
        receivedValues.Clear(); // limpa os valores recebidos

        UpdateUI();
        UpdateLevelUI(); // Atualiza o texto do nível
        feedbackPanel.SetActive(false); // Esconde a mensagem no início
        DisplayRandomNotes();
    }

    private void UpdateAvailableDenominations(int level)
    {
        int[] allCoins = { 1, 2 };

        // Níveis 1 a 5 → apenas moedas
        if (level <= 5)
        {
            availableDenominations = new List<int>(allCoins);
        }

        // Níveis 6+ → moedas + notas
        else
        {
            int[] allNotes = { 5, 10, 20 };
            int[] selectedNotes = allNotes.OrderBy(x => Random.value).Take(2).ToArray();

            availableDenominations = new List<int>(selectedNotes);
            availableDenominations.Add(allCoins[Random.Range(0, allCoins.Length)]);
        }
    }

    private void GetRequestedValueMatchingOnly()
    {
        requestedValues.Clear();

        int numberOfRequestedValues = 1;
        if (currentLevel >= 11)
            numberOfRequestedValues = 3;
        else if (currentLevel >= 6)
            numberOfRequestedValues = 2;

        // Garante que há denominações suficientes para escolher
        List<int> possibleValues = new List<int>(availableDenominations);

        // Se não houver valores suficientes, repete alguns
        for (int i = 0; i < numberOfRequestedValues; i++)
        {
            int selectedValue;

            if (possibleValues.Count > 0)
            {
                int index = Random.Range(0, possibleValues.Count);
                selectedValue = possibleValues[index];
                possibleValues.RemoveAt(index);
            }
            else
            {
                // Repetir valores se não houver mais disponíveis
                selectedValue = availableDenominations[Random.Range(0, availableDenominations.Count)];
            }

            requestedValues.Add(selectedValue);
        }

        // Como agora são vários valores, vamos pedir apenas o primeiro como base para `requestedAmount`
        requestedAmount = requestedValues[0];

        UpdateRequestedSprites(requestedValues);
    }

    private void UpdateRequestedSprites(List<int> values)
    {
        for (int i = 0; i < requestedAmountImages.Count; i++)
        {
            if (i < values.Count)
            {
                int value = values[i];
                requestedAmountImages[i].enabled = true;

                switch (value)
                {
                    case 1: requestedAmountImages[i].sprite = requestedSprites[0]; break;
                    case 2: requestedAmountImages[i].sprite = requestedSprites[1]; break;
                    case 5: requestedAmountImages[i].sprite = requestedSprites[2]; break;
                    case 10: requestedAmountImages[i].sprite = requestedSprites[3]; break;
                    case 20: requestedAmountImages[i].sprite = requestedSprites[4]; break;
                    default: requestedAmountImages[i].enabled = false; break;
                }
            }
            else
            {
                requestedAmountImages[i].enabled = false;
            }
        }
    }

    private void UpdateUI()
    {
        //currentTotalText.text = "";
    }

    private void UpdateLevelUI()
    {
        if (levelText != null)
        {
            levelText.text = "Nível: " + currentLevel.ToString();
        }
    }

    public void ReceberNota(int valor)
    {
        if (interactionLocked) return;

        if (requestedValues.Contains(valor))
        {
            receivedValues.Add(valor);
            requestedValues.Remove(valor);

            // Atualiza os checks visuais
            ShowFeedbackChecks(receivedValues);

            // Atualiza barra de progresso
            float progress = (float)receivedValues.Count / (float)(receivedValues.Count + requestedValues.Count);
            progressBarFill.fillAmount = progress;

            // Se já entregou todos os valores pedidos
            if (requestedValues.Count == 0)
            {
                interactionLocked = true;
                StartCoroutine(HandleCorrectAnswer());
            }
        }
        else
        {
            interactionLocked = true;
            HideAllFeedbackChecks();
            StartCoroutine(HandleWrongAnswer());
        }
    }



    private IEnumerator HandleCorrectAnswer()
    {
        Debug.Log("HandleCorrectAnswer triggered");

        if (correctAnimator != null)
        {
            correctAnimator.SetTrigger("ShowCorrect");
        }

        if (correctSound != null)
        {
            audioSource.PlayOneShot(correctSound, 2.0f); // valor entre 0.0 e 1.0
        }

        yield return new WaitForSeconds(3.7f); // Tempo de duração dos checks

        HideAllFeedbackChecks(); // Esconde os checks
        currentLevel++;
        StartNewRound();
    }

    private IEnumerator HandleWrongAnswer()
    {
        ShowMessage("Errado! Não corresponde ao valor pedido.", Color.red);

        if (wrongSound != null)
        {
            audioSource.PlayOneShot(wrongSound);
        }

        yield return new WaitForSeconds(2f);

        interactionLocked = false;
        feedbackPanel.SetActive(false);
    }

    private void ShowMessage(string message, Color color)
    {
        messageText.text = message;
        messageText.color = color;
        feedbackPanel.SetActive(true);
    }

    private void DisplayRandomNotes()
    {
        // Limpa notas anteriores
        foreach (var item in currentMoneyItems)
        {
            Destroy(item);
        }
        currentMoneyItems.Clear();

        var noteValues = availableDenominations.Where(v => v >= 5).ToList();
        var coinValues = availableDenominations.Where(v => v == 1 || v == 2).ToList();

        // Garante que o valor pedido está incluído
        if (!availableDenominations.Contains(requestedAmount))
        {
            availableDenominations.Add(requestedAmount);
            if (requestedAmount >= 5)
                noteValues.Add(requestedAmount);
            else
                coinValues.Add(requestedAmount);
        }

        // Posicionamento de notas
        if (notePositions.Count == 0 && noteValues.Count > 0)
        {
            notePositions.Clear();
            bool shuffle = Random.value > 0.5f;

            if (noteValues.Count >= 2)
            {
                notePositions[noteValues[0]] = shuffle ? leftNoteSlot : rightNoteSlot;
                notePositions[noteValues[1]] = shuffle ? rightNoteSlot : leftNoteSlot;
            }
            else if (noteValues.Count == 1)
            {
                notePositions[noteValues[0]] = leftNoteSlot;
            }
        }

        foreach (var noteValue in noteValues.Distinct())
        {
            GameObject prefab = GetNotePrefab(noteValue);
            if (prefab != null && notePositions.ContainsKey(noteValue))
            {
                Transform slot = notePositions[noteValue];
                GameObject instance = Instantiate(prefab, slot);
                instance.transform.localPosition = Vector3.zero;
                currentMoneyItems.Add(instance);
            }
        }

        // Moedas
        if (currentLevel <= 5)
        {
            Transform coinSlot = requestedAmount == 1 ? coin1Slot : coin2Slot;
            GameObject coinPrefab = requestedAmount == 1 ? coin1Prefab : coin2Prefab;

            if (coinSlot != null && coinPrefab != null)
            {
                GameObject instance = Instantiate(coinPrefab, coinSlot);
                instance.transform.localPosition = Vector3.zero;
                currentMoneyItems.Add(instance);
            }
        }
        else
        {
            // Mostrar todas as moedas disponíveis (incluindo a pedida)
            if (coinValues.Contains(1) && coin1Slot != null && coin1Prefab != null)
            {
                GameObject coin1Instance = Instantiate(coin1Prefab, coin1Slot);
                coin1Instance.transform.localPosition = Vector3.zero;
                currentMoneyItems.Add(coin1Instance);
            }

            if (coinValues.Contains(2) && coin2Slot != null && coin2Prefab != null)
            {
                GameObject coin2Instance = Instantiate(coin2Prefab, coin2Slot);
                coin2Instance.transform.localPosition = Vector3.zero;
                currentMoneyItems.Add(coin2Instance);
            }
        }
    }


    private GameObject GetNotePrefab(int value)
    {
        switch (value)
        {
            case 5: return note5Prefab;
            case 10: return note10Prefab;
            case 20: return note20Prefab;
            default: return null;
        }
    }

    public void SetMusicVolume(float volume)
    {
        audioSource.volume = volume;
        PlayerPrefs.SetFloat(MusicVolumeKey, volume);

        // Atualiza o texto da percentagem
        if (musicVolumePercentageText != null)
        {
            int percent = Mathf.RoundToInt(volume * 100f);
            musicVolumePercentageText.text = Mathf.RoundToInt((musicVolumeSlider.value / musicVolumeSlider.maxValue) * 100) + "%";
        }
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void RetryLevel()
    {
        // Mantém o nível atual e reinicia a rodada
        interactionLocked = false;

        UpdateUI();
        feedbackPanel.SetActive(false);

        StartNewRound();
    }

    public void QuitToMenu()
    {
        // Resetar o estado do minigame para o ínicio
        currentLevel = 1;
        interactionLocked = false;
        UpdateUI();
        feedbackPanel.SetActive(false);
    }
    
    private void ShowFeedbackChecks(List<int> values)
    {
        HideAllFeedbackChecks(); // Limpa antes de mostrar novos

        for (int i = 0; i < values.Count && i < feedbackCheckSlots.Length; i++)
        {
            if (feedbackCheckPrefab != null && feedbackCheckSlots[i] != null)
            {
                GameObject check = Instantiate(feedbackCheckPrefab, feedbackCheckSlots[i]);
                check.transform.localPosition = Vector3.zero;
                currentFeedbackChecks.Add(check);
            }
        }
    }

    private void HideAllFeedbackChecks()
    {
        foreach (var check in currentFeedbackChecks)
        {
            if (check != null)
            {
                Destroy(check);
            }
        }
        currentFeedbackChecks.Clear();
    }
}