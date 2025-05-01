using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{
    public List<QuestionAndAnswers> QnA; // Lista de perguntas e respostas
    public GameObject[] options; // Botões de resposta
    public int currentQuestion; // Índice da pergunta atual

    public GameObject Quizpanel; // Painel do quiz (ativo durante o jogo)
    public GameObject GOPanel; // Painel de fim de jogo

    public Text QuestionTxt; // Texto da pergunta
    public Text ScoreTxt; // Texto de pontuação final

    int totalQuestions = 0; // Número total de perguntas no início
    public int score; // Pontuação do jogador

    private void Start()
    {
        totalQuestions = QnA.Count; // Guarda número inicial de perguntas
        GOPanel.SetActive(false); // Esconde o painel de fim de jogo
        GenerateQuestion(); // Gera a primeira pergunta
    }

    // Recarrega a cena atual para recomeçar o quiz
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Mostra o painel de Game Over com a pontuação final
    void GameOver()
    {
        Quizpanel.SetActive(false);
        GOPanel.SetActive(true);
        ScoreTxt.text = score + "/" + totalQuestions; // Exibe pontuação
    }

    // Chamada quando o jogador acerta a resposta
    public void Correct()
    {
        score += 1; // Aumenta pontuação
        QnA.RemoveAt(currentQuestion); // Remove pergunta atual da lista
        GenerateQuestion(); // Gera próxima pergunta
    }

    // Chamada quando o jogador erra a resposta
    public void Wrong()
    {
        QnA.RemoveAt(currentQuestion); // Apenas remove a pergunta
        GenerateQuestion(); // Gera próxima
    }

    // Define as respostas nos botões e marca qual é a correta
    public void SetAnswers()
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<AnswerScript>().isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<Text>().text = QnA[currentQuestion].Answers[i];

            if(QnA[currentQuestion].CorrectAnswer == i+1) // Verifica se esta é a resposta correta
            {
                options[i].GetComponent<AnswerScript>().isCorrect = true;
            }
        }

    }

    // Gera uma nova pergunta aleatória ou termina o quiz se acabaram
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
