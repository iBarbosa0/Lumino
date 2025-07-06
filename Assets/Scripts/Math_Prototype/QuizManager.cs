using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{
    public List<QuestionAndAnswers> QnA; // Lista de perguntas e respostas
    public GameObject[] options; // Bot�es de resposta
    public int currentQuestion; // �ndice da pergunta atual

    public GameObject Quizpanel; // Painel do quiz (ativo durante o jogo)
    public GameObject GOPanel; // Painel de fim de jogo

    public Text QuestionTxt; // Texto da pergunta
    public Text ScoreTxt; // Texto de pontua��o final

    int totalQuestions = 0; // N�mero total de perguntas no in�cio
    public int score; // Pontua��o do jogador

    private void Start()
    {
        totalQuestions = QnA.Count; // Guarda n�mero inicial de perguntas
        GOPanel.SetActive(false); // Esconde o painel de fim de jogo
        GenerateQuestion(); // Gera a primeira pergunta
    }

    // Recarrega a cena atual para recome�ar o quiz
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Mostra o painel de Game Over com a pontua��o final
    void GameOver()
    {
        Quizpanel.SetActive(false);
        GOPanel.SetActive(true);
        ScoreTxt.text = score + "/" + totalQuestions; // Exibe pontua��o
    }

    // Chamada quando o jogador acerta a resposta
    public void Correct()
    {
        score += 1; // Aumenta pontua��o
        QnA.RemoveAt(currentQuestion); // Remove pergunta atual da lista
        GenerateQuestion(); // Gera pr�xima pergunta
    }

    // Chamada quando o jogador erra a resposta
    public void Wrong()
    {
        QnA.RemoveAt(currentQuestion); // Apenas remove a pergunta
        GenerateQuestion(); // Gera pr�xima
    }

    // Define as respostas nos bot�es e marca qual � a correta
    public void SetAnswers()
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<AnswerScript>().isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<Text>().text = QnA[currentQuestion].Answers[i];

            if(QnA[currentQuestion].CorrectAnswer == i+1) // Verifica se esta � a resposta correta
            {
                options[i].GetComponent<AnswerScript>().isCorrect = true;
            }
        }

    }

    // Gera uma nova pergunta aleat�ria ou termina o quiz se acabaram
    public void GenerateQuestion()
    {
        if(QnA.Count > 0)
        {
            currentQuestion = Random.Range(0, QnA.Count);

            QuestionTxt.text = QnA[currentQuestion].Question;
            SetAnswers();
        }
        else
        {
            Debug.Log("Out of Questions");
            GameOver();
        }
    }
}
