using System;
using Unity.VisualScripting;
using UnityEngine;

public class BubbleLetter : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Sprite[] _spritesArray;
    public bool WasItPoped = true;
    private int LetterPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
          LetterPosition = UnityEngine.Random.Range(0, 25);
         _spriteRenderer = GetComponent<SpriteRenderer>();
         _spriteRenderer.sprite = LetterManager.LetterManagerInstance.SpritesArray[LetterPosition];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        if (WasItPoped == false)
        {
            return;
        }
        
        for (int i = 0; i < LetterManager.LetterManagerInstance.ChosenWord.Length; i++)
        {
            if (LetterPosition ==LetterManager.LetterManagerInstance.BubbleLetterBox[i].GetComponent<BubbleLetterBox>().letterPosition )
            {
                LetterManager.LetterManagerInstance.BubbleLetterBox[i].transform.Find("Letter").GetComponent<SpriteRenderer>().sprite = LetterManager.LetterManagerInstance.SpritesArray[LetterPosition];
                Debug.Log("Letter Position of the bubble " + LetterPosition);
                Debug.Log("letter position " + LetterManager.LetterManagerInstance.AlphabetPosition[i]);
                Debug.Log("Correct letter");
               // break;
            }
        }
    }
}
