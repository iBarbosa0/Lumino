[System.Serializable] // Permite que esta classe apare�a no Inspector do Unity
public class QuestionAndAnswers
{
    public string Question; // Texto da pergunta
    public string[] Answers; // Lista de respostas poss�veis (geralmente 4)
    public int CorrectAnswer; // �ndice (base 1) da resposta correta
};
