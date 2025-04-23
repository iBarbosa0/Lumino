using UnityEngine;

public class BubbleLetterBox : MonoBehaviour
{
    public int letterPosition;
    public void Initialize(int LetterPosition)
    {
        letterPosition = LetterPosition;
        Debug.Log("LetterPosition "+ letterPosition);
    }
 
}
