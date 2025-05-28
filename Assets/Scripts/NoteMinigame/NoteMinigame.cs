using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NoteMinigame : MonoBehaviour
{
    public Text requestedAmountText; // Texto ao lado da nota verde
    public Text currentTotalText;    // Texto que mostra quanto o jogador já deu
    public Text messageText;         // Mensagem de feedback ("Acertou!" etc.)

    public Button validateButton;    // Botão para confirmar total
    public Button resetButton;       // Botão para reiniciar total

    public int requestedAmount = 20; // Valor pedido pela senhora da caixa (nível 1 = 20)
    private int currentTotal = 0;
    private bool interactionLocked = false;

    private int currentLevel = 1;
    private int lastRequestedAmount = -1;

    private void Start()
    {
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

        messageText.gameObject.SetActive(false); // Oculta mensagem até validação

    }

    public void OnValidPressed()
    {
        if (interactionLocked) return;

        validateButton.interactable = false;
        resetButton.interactable= false;
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
        messageText.gameObject.SetActive(false);

        validateButton.interactable = false;
        resetButton.interactable= false;
    }

    private void StartNewRound()
    {
        requestedAmount = GetAmountBasedOnLevel(currentLevel);

        currentTotal = 0;
        interactionLocked = false;

        UpdateUI();
        messageText.gameObject.SetActive(false); // Esconde a mensagem no início

        validateButton.interactable = false;
        resetButton.interactable = false;
    }

    private int GetAmountBasedOnLevel(int level)
    {
        int[] options;

        // Valores criativos, abaixo ou iguais a 20
        int[] easy = { 6, 7, 9, 11, 14, 15, 19, 20 };               // Nível 1-5
        int[] medium = { 5, 8, 10, 12, 13, 16, 17, 18 };            // Nível 6-10
        int[] hard = { 3, 4, 6, 7, 9, 11, 13, 14, 15, 19 };         // Nível 11-15
        int[] expert = { 1, 2, 3, 4, 6, 7, 8, 11, 12, 17, 18, 20 }; // Nível 15+

        if (level <= 5) options = easy;
        else if (level <= 10) options = medium;
        else if (level <= 15) options = hard;
        else options = expert;

        // Evita repetir o mesmo valor da ronda anterior
        if (lastRequestedAmount != -1 && options.Length > 1)
        {
            options = System.Array.FindAll(options, val => val != lastRequestedAmount);
        }

        int selected = options[Random.Range(0, options.Length)];
        lastRequestedAmount = selected;
        return selected;
    }

    private void UpdateUI()
    {
        requestedAmountText.text = requestedAmount.ToString();
        currentTotalText.text = "Total dado: " + currentTotal.ToString();
    }

    private void ShowMessage(string message, Color color)
    {
        messageText.text = message;
        messageText.color = color;
        messageText.gameObject.SetActive(true);
    }

    private IEnumerator HandleCorrectAnswer()
    {
        ShowMessage("Certo! Pagamento Concluído.", Color.green);
        yield return new WaitForSeconds(2f);
        currentLevel++;
        StartNewRound();
    }

    private IEnumerator HandleWrongAnswer()
    {
        currentLevel = 1; // Reinicia para o nível 1 ao errar
        ShowMessage("Ups! Deste dinheiro a mais.", Color.red);
        yield return new WaitForSeconds(2f);
        StartNewRound();
    }

    private IEnumerator ShowTemporaryMessasge(string message, Color color, float duration)
    {
        ShowMessage(message, color);
        yield return new WaitForSeconds(duration);
        messageText.gameObject.SetActive(false);

        interactionLocked = false;

        // Reativa os botões se o jogador já tiver dado algo
        if (currentTotal > 0)
        {
            validateButton.interactable = true;
            resetButton.interactable = true;
        }
    }
}
