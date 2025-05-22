using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NoteMinigame : MonoBehaviour
{
    public Text requestedAmountText; // Texto ao lado da nota verde
    public Text currentTotalText;    // Texto que mostra quanto o jogador já deu
    public Text messageText;         // Mensagem de feedback ("Acertou!" etc.)

    public int requestedAmount = 20; // Valor pedido pela senhora da caixa (nível 1 = 20)
    private int currentTotal = 0;

    private void Start()
    {
        StartNewRound();    
    }

    public void AddNoteValue(int value)
    {
        currentTotal += value;
        UpdateUI();

        if (currentTotal == requestedAmount)
        {
            StartCoroutine(HandleCorrectAnswer());
            // Método para avançar de nível
        }
        else if (currentTotal > requestedAmount)
        {
            StartCoroutine(HandleWrongAnswer());
        }
        else
        {
            ShowMessage("Ainda falta...", new Color(1f, 0.64f, 0f)); // Amarelo alaranjado
        }
    }

    private void StartNewRound()
    {
        // Escolhe aleatoriamente entre 5, 10, 20 ou 50
        int[] possibleAmounts = { 5, 10, 20, 50 };
        requestedAmount = possibleAmounts[Random.Range(0, possibleAmounts.Length)];

        currentTotal = 0;
        messageText.gameObject.SetActive(false); // Esconde a mensagem no início
        UpdateUI();
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
        StartNewRound();
    }

    private IEnumerator HandleWrongAnswer()
    {
        ShowMessage("Ups! Deste dinheiro a mais.", Color.red);
        yield return new WaitForSeconds(2f);
        StartNewRound();
    }
}
