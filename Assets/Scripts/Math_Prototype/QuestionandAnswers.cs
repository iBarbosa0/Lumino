[System.Serializable] // Permite que esta classe apareça no Inspector do Unity
public class QuestionAndAnswers
{
    public string Question; // Texto da pergunta
    public string[] Answers; // Lista de respostas possíveis (geralmente 4)
    public int CorrectAnswer; // Índice (base 1) da resposta correta
};
