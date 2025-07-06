using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerScript : MonoBehaviour
{
    public bool isCorrect = false; // Define se este botão é a resposta correta
    public QuizManager quizManager; // Referência ao QuizManager para chamar métodos

    // Chamado quando o jogador clica nesta resposta
    public void Answer()
    {
        if (isCorrect)
        {
            Debug.Log("Correct Answer"); // Log para feedback no console
            quizManager.Correct(); // Informa ao quiz que acertou
        }
        else
        {
            Debug.Log("Wrong Answer"); // Log de erro
            quizManager.Wrong(); // Informa ao quiz que errou
        }
    }
}