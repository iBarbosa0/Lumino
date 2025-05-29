using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NoteMinigame : MonoBehaviour
{
    public Text requestedAmountText; // Texto ao lado da nota verde
    public Text currentTotalText;    // Texto que mostra quanto o jogador já deu
    public Text messageText;         // Mensagem de feedback ("Acertou!" etc.)
    public Text levelText;           // Texto para mostrar o nível atual

    public Button validateButton;    // Botão para confirmar total
    public Button resetButton;       // Botão para reiniciar total

    // Botões das denominações
    public Button button1;
    public Button button2;
    public Button button5;
    public Button button10;
    public Button button20;

    public Transform leftNoteSlot;
    public Transform rightNoteSlot;

    public GameObject note5Prefab;
    public GameObject note10Prefab;
    public GameObject note20Prefab;

    public GameObject feedbackPanel;

    public int requestedAmount = 20; // Valor pedido pela senhora da caixa (nível 1 = 20)
    private int currentTotal = 0;
    private bool interactionLocked = false;

    private int currentLevel = 1;
    private int lastRequestedAmount = -1;

    public AudioClip correctSound;    // Som de acerto
    public AudioClip wrongSound;      // Som de erro
    public AudioClip backgroundMusic; // Música de fundo
    private AudioSource audioSource;  // Componente AudioSource

    public List<int> availableDenominations = new List<int>();
    private List<GameObject> currentNotes = new List<GameObject>();

    private void Start()
    {
        // Configuração AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.loop = true; // Faz a música tocar em loop
        audioSource.playOnAwake = false;
        audioSource.volume = 0.5f; // Ajusta o volume (o a 1)

        audioSource.Play();

        StartNewRound();
    }

    public void AddNoteValue(int value)
    {
        if (interactionLocked) return;

        currentTotal += value;
        UpdateUI();

        // Ativa botões ao adicionar nota
        validateButton.interactable = true;
        resetButton.interactable = true;
        feedbackPanel.SetActive(false); // Oculta mensagem até validação
    }

    public void OnValidPressed()
    {
        if (interactionLocked) return;

        validateButton.interactable = false;
        resetButton.interactable = false;
        interactionLocked = true;

        if (currentTotal == requestedAmount)
        {
            StartCoroutine(HandleCorrectAnswer());
        }
        else if (currentTotal > requestedAmount)
        {
            StartCoroutine(HandleWrongAnswer());
        }
        else
        {
            StartCoroutine(ShowTemporaryMessasge("Ainda falta...", new Color(1f, 0.64f, 0f), 2f));
        }
    }

    public void OnResetPressed()
    {
        if (interactionLocked) return;

        currentTotal = 0;
        UpdateUI();
        feedbackPanel.SetActive(false);

        validateButton.interactable = false;
        resetButton.interactable = false;
    }

    private void StartNewRound()
    {
        requestedAmount = GetAmountBasedOnLevel(currentLevel);
        currentTotal = 0;
        interactionLocked = false;

        UpdateUI();
        UpdateLevelUI(); // Atualiza o texto do nível
        feedbackPanel.SetActive(false); // Esconde a mensagem no início

        validateButton.interactable = false;
        resetButton.interactable = false;

        UpdateAvailableButtons(); // << ativa/desativa botões conforme as denominações
        DisplayRandomNotes();
    }

    private int GetAmountBasedOnLevel(int level)
    {
        int[] allNotes = { 5, 10, 20 };
        int[] allCoins = { 1, 2 };

        // Escolhe 2 notas aleatórias
        int[] selectedNotes = allNotes.OrderBy(x => Random.value).Take(2).ToArray();

        // Escolhe 1 moeda aleatória
        int selectedCoin = allCoins[Random.Range(0, allCoins.Length)];

        // Guarda denominações disponíveis
        availableDenominations = selectedNotes.Concat(new int[] { selectedCoin }).ToList();

        // Gera os valores possíveis até 20
        List<int> possibleValues = GeneratePossibleValues(availableDenominations, 20);

        // Filtra por dificuldade
        List<int> filtered = FilterByLevelDifficulty(possibleValues, level);

        // Evita repetir o mesmo valor da ronda anterior
        if (lastRequestedAmount != -1 && filtered.Count > 1)
        {
            filtered = filtered.Where(v => v != lastRequestedAmount).ToList();
        }

        int selected = filtered[Random.Range(0, filtered.Count)];
        lastRequestedAmount = selected;
        return selected;
    }

    private List<int> GeneratePossibleValues(List<int> denominations, int max)
    {
        HashSet<int> values = new HashSet<int>();

        for (int a = 0; a <= 10; a++)
        {
            for (int b = 0; b <= 10; b++)
            {
                for (int c = 0; c <= 10; c++)
                {
                    int total = a * denominations[0] + b * denominations[1] + c * denominations[2];
                    if (total > 0 && total <= max)
                    {
                        values.Add(total);
                    }
                }
            }
        }

        return values.ToList();
    }

    private List<int> FilterByLevelDifficulty(List<int> values, int level)
    {
        if (level <= 5)
        {
            return values.Where(v => v >= 3 && v <= 10).ToList();
        }
        else if (level <= 10)
        {
            return values.Where(v => v >= 11 && v <= 15).ToList();
        }
        else if (level <= 15)
        {
            return values.Where(v => v >= 16 && v <= 19).ToList();
        }
        else
        {
            return values.Where(v => new int[] { 2, 3, 5, 7, 9, 11, 13, 17, 19, 20 }.Contains(v)).ToList();
        }
    }

    private void UpdateUI()
    {
        requestedAmountText.text = requestedAmount.ToString();
        currentTotalText.text = "Total dado: " + currentTotal.ToString();
    }

    private void UpdateLevelUI()
    {
        if (levelText != null)
        {
            levelText.text = "Nível: " + currentLevel.ToString();
        }
    }

    private void ShowMessage(string message, Color color)
    {
        messageText.text = message;
        messageText.color = color;
        feedbackPanel.SetActive(true);
    }

    private IEnumerator HandleCorrectAnswer()
    {
        ShowMessage("Certo! Pagamento concluído.", Color.green);
        if (correctSound != null)
        {
            audioSource.PlayOneShot(correctSound);
        }

        yield return new WaitForSeconds(2f);
        currentLevel++;
        StartNewRound();
    }

    private IEnumerator HandleWrongAnswer()
    {
        currentLevel = 1; // Reinicia para o nível 1 ao errar
        ShowMessage("Ups! Deste dinheiro a mais.", Color.red);
        if (wrongSound != null)
        {
            audioSource.PlayOneShot(wrongSound);
        }

        yield return new WaitForSeconds(2f);
        StartNewRound();
    }

    private IEnumerator ShowTemporaryMessasge(string message, Color color, float duration)
    {
        ShowMessage(message, color);
        if (wrongSound != null)
        {
            audioSource.PlayOneShot(wrongSound);    
        }

        yield return new WaitForSeconds(duration);
        feedbackPanel.SetActive(false);

        interactionLocked = false;

        // Reativa os botões se o jogador já tiver dado algo
        if (currentTotal > 0)
        {
            validateButton.interactable = true;
            resetButton.interactable = true;
        }
    }

    private void UpdateAvailableButtons()
    {
        button1.gameObject.SetActive(availableDenominations.Contains(1));
        button2.gameObject.SetActive(availableDenominations.Contains(2));
        button5.gameObject.SetActive(availableDenominations.Contains(5));
        button10.gameObject.SetActive(availableDenominations.Contains(10));
        button20.gameObject.SetActive(availableDenominations.Contains(20));

    }

    private void DisplayRandomNotes()
    {
        // Limpa notas anteriores
        foreach (var note in currentNotes)
        {
            Destroy(note);
        }
        currentNotes.Clear();

        // Pega apenas notas (>= 5)
        var noteValues = availableDenominations.Where(v => v >= 5).ToList();

        if (noteValues.Count != 2)
        {
            Debug.LogWarning("Esperado 2 notas para mostrar");
            return;
        }

        var shuffled = noteValues.OrderBy(x => Random.value).ToList();

        for (int i = 0; i < shuffled.Count; i++)
        {
            int noteValue = shuffled[i];
            GameObject prefab = GetNotePrefab(noteValue);
            if (prefab != null)
            {
                Transform slot = (i == 0) ? leftNoteSlot : rightNoteSlot;
                GameObject instance = Instantiate(prefab, slot);
                instance.transform.localPosition = Vector3.zero;

                // Adiciona a função AddNoteValue ao botão instanciado
                Button button = instance.GetComponent<Button>();
                if (button != null)
                {
                    int capturedValue = noteValue; // Captura o valor corretamente para evitar closure bugs
                    button.onClick.AddListener(() => AddNoteValue(capturedValue));
                }
                else
                {
                    Debug.LogWarning("O prefab não tem componente Button!");
                }


                currentNotes.Add(instance);
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
}
