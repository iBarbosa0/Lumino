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
    public Text gameOverHighscoreText; // Texto para mostrar highscore no Game Over

    public Button startButton;       // Botão que inicia o minigame
    public Button backButton;        // Botão que volta ao menu principal
    public Button validateButton;    // Botão para confirmar total
    public Button resetButton;       // Botão para reiniciar total
    public Button retryButton;       // Botão "Tentar outra vez"
    public Button quitButton;        // Botão "sair"
    
    public Transform leftNoteSlot;
    public Transform rightNoteSlot;
    public Transform coin1Slot;
    public Transform coin2Slot;

    public GameObject coin1Prefab;
    public GameObject coin2Prefab;
    public GameObject note5Prefab;
    public GameObject note10Prefab;
    public GameObject note20Prefab;

    public GameObject feedbackPanel;
    public GameObject mainMenuPanel; // Painel do menu principal
    public GameObject settingsPanel; // Painel de configurações
    public GameObject gameOverPanel; // Painel Game Over

    public Slider musicVolumeSlider; // Slider do volume de música

    public int requestedAmount = 20; // Valor pedido pela senhora da caixa (nível 1 = 20)
    private int currentTotal = 0;
    private bool interactionLocked = false;

    private int currentLevel = 1;
    private int lastRequestedAmount = -1;
    private int highscore = 1;        // maior nível alcançado

    public AudioClip correctSound;    // Som de acerto
    public AudioClip wrongSound;      // Som de erro
    public AudioClip backgroundMusic; // Música de fundo
    private AudioSource audioSource;  // Componente AudioSource

    public List<int> availableDenominations = new List<int>();
    private List<GameObject> currentMoneyItems = new List<GameObject>();

    private const string MusicVolumeKey = "MusicVolume";
    private const string NoteMinigameHighscoreKey = "NoteMinigame_Highscore"; // Chave única para highscore deste minigame

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

        // Conecta evento do slider
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);

        // Carrega o highscore salvo com a chave exclusiva
        highscore = PlayerPrefs.GetInt(NoteMinigameHighscoreKey, 1);

        // Conecta os botões do Game Over
        if (retryButton != null)
        {
            retryButton.onClick.AddListener(RetryLevel);
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitToMenu);
        }

        gameOverPanel.SetActive(false); // Inicalmente o painel de game over deve estar inativado
        mainMenuPanel.SetActive(true); // Mostra o menu
    }

    public void StartMinigame()
    {
        mainMenuPanel.SetActive(false); // Esconde o menu
        StartNewRound();               // Inicia o jogo
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
            StartCoroutine(ShowTemporaryMessage("Ainda falta...", new Color(1f, 0.64f, 0f), 2f));
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

        // Recria as notas
        DisplayRandomNotes();
    }

    private void StartNewRound()
    {
        notePositions.Clear();
        requestedAmount = GetAmountBasedOnLevel(currentLevel);
        currentTotal = 0;
        interactionLocked = false;

        UpdateUI();
        UpdateLevelUI(); // Atualiza o texto do nível
        feedbackPanel.SetActive(false); // Esconde a mensagem no início

        validateButton.interactable = false;
        resetButton.interactable = false;

        DisplayRandomNotes();
    }

    private int GetAmountBasedOnLevel(int level)
    {
        int[] allNotes = { 5, 10, 20 };
        int[] selectedNotes = allNotes.OrderBy(x => Random.value).Take(2).ToArray();

        // Começa apenas com notas
        availableDenominations = new List<int>(selectedNotes);

        // A partir do nível 6, começa a introduzir moedas
        if (level >= 6)
        {
            int[] allCoins = { 1, 2 };
            int selectedCoin = allCoins[Random.Range(0, allCoins.Length)];
            availableDenominations.Add(selectedCoin);
        }

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

        int count = denominations.Count;
        if (count == 0) return values.ToList(); // Evita erros se lista estiver vazia

        // Limite prático de combinações por denominação
        int limit = 10;

        // Gera todas as combinações possíveis com base no número de denominações
        void Recurse(int index, int currentSum)
        {
            if (index >= count)
            {
                if (currentSum > 0 && currentSum <= max)
                {
                    values.Add(currentSum);
                }
                return;
            }

            for (int i = 0; i <= limit; i++)
            {
                int nextSum = currentSum + i * denominations[index];
                if (nextSum > max) break;
                Recurse(index + 1, nextSum);
            }
        }

        Recurse(0, 0);
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

        // Atualiza o highscore
        if (currentLevel > highscore)
        {
            highscore = currentLevel;
            PlayerPrefs.SetInt(NoteMinigameHighscoreKey, highscore);
        }

        StartNewRound();
    }

    private IEnumerator HandleWrongAnswer()
    {
        ShowMessage("Ups! Deste dinheiro a mais.", Color.red);
        if (wrongSound != null)
        {
            audioSource.PlayOneShot(wrongSound);
        }

        yield return new WaitForSeconds(2f);

        // Mostra painel game over
        feedbackPanel.SetActive(false);
        gameOverPanel.SetActive(true);

        // Mostra o highscore no painel
        if (gameOverHighscoreText != null)
        {
            gameOverHighscoreText.text = "Pontuação Máxima " + highscore.ToString();
        }
    }

    private IEnumerator ShowTemporaryMessage(string message, Color color, float duration)
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

    private void DisplayRandomNotes()
    {
        // Limpa notas anteriores
        foreach (var item in currentMoneyItems)
        {
            Destroy(item);
        }
        currentMoneyItems.Clear();

        // Pega apenas notas (>= 5)
        var noteValues = availableDenominations.Where(v => v >= 5).ToList();

        if (noteValues.Count != 2)
        {
            Debug.LogWarning("Esperado 2 notas para mostrar");
            return;
        }

        // Se for a primeira vez nesta ronda, define a posição original:
        if (notePositions.Count == 0)
        {
            notePositions.Clear();
            // aleatoriamente atribui nota ao slot esquerdo ou direito só uma vez
            bool shuffle = Random.value > 0.5f;
            notePositions[noteValues[0]] = shuffle ? leftNoteSlot : rightNoteSlot;
            notePositions[noteValues[1]] = shuffle ? rightNoteSlot : leftNoteSlot;
        }

        // Para cada nota, instancia-a na posição original
        foreach (var noteValue in noteValues)
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

        // Instancia moedas (se disponíveis)
        if (availableDenominations.Contains(1) && coin1Slot != null && coin1Prefab != null)
        {
            GameObject coin1Instance = Instantiate(coin1Prefab, coin1Slot);
            coin1Instance.transform.localPosition = Vector3.zero;
            currentMoneyItems.Add(coin1Instance);
        }

        if (availableDenominations.Contains(2) && coin2Slot != null && coin2Prefab != null)
        {
            GameObject coin2Instance = Instantiate(coin2Prefab, coin2Slot);
            coin2Instance.transform.localPosition = Vector3.zero;
            currentMoneyItems.Add(coin2Instance);
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

    private GameObject GetCoinPrefab(int value)
    {
        switch (value)
        {
            case 1: return coin1Prefab;
            case 2: return coin2Prefab;
            default : return null;
        }
    }


    public void SetMusicVolume(float volume)
    {
        audioSource.volume = volume;
        PlayerPrefs.SetFloat(MusicVolumeKey, volume);
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void Settings()
    {
        settingsPanel.SetActive(false);
    }

    public void RetryLevel()
    {
        // Esconde o painel game over
        gameOverPanel.SetActive(false);

        // Mantém o nível atual e reinicia a rodada
        currentTotal = 0;
        interactionLocked = false;

        UpdateUI();
        feedbackPanel.SetActive(false);

        validateButton.interactable = false;
        resetButton.interactable = false;

        StartNewRound();
    }

    public void QuitToMenu()
    {
        // Esconde o game over e mostra o menu principal
        gameOverPanel.SetActive(false);
        mainMenuPanel.SetActive(true);

        // Resetar o estado do minigame para o ínicio
        currentLevel = 1;
        currentTotal = 0;
        interactionLocked = false;

        UpdateUI();
        feedbackPanel.SetActive(false);

        validateButton.interactable = false;
        resetButton.interactable = false;
    }

    public void ReceberNota(int valor)
    {
        if (interactionLocked) return;

        currentTotal += valor;
        UpdateUI();

        validateButton.interactable = true;
        resetButton.interactable = true;
        feedbackPanel.SetActive(false); // Esconde mensagem até validação
    }
}
