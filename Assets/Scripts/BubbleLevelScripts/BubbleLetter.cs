using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class BubbleLetter : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Sprite[] _spritesArray;
    public bool WasItPoped = true;
    private int LetterPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (UnityEngine.Random.Range(0, 1) == 0)
        {
           LetterPosition =  LetterManager.LetterManagerInstance.AlphabetPosition[UnityEngine.Random.Range(0, LetterManager.LetterManagerInstance.AlphabetPosition.Length)];
        }
        else
        {
            LetterPosition = UnityEngine.Random.Range(0, 25);
        }
         
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
                //LetterManager.LetterManagerInstance.BubbleLetterBox[i].transform.Find("Letter").GetComponent<SpriteRenderer>().sprite = LetterManager.LetterManagerInstance.SpritesArray[LetterPosition];
                SpriteRenderer _spriteRenderer = LetterManager.LetterManagerInstance.BubbleLetterBox[i].transform.Find("Letter").GetComponent<SpriteRenderer>();
                _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 1f);
                Debug.Log("Letter Position of the bubble " + LetterPosition);
                Debug.Log("letter position " + LetterManager.LetterManagerInstance.AlphabetPosition[i]);
                Debug.Log("Correct letter");
                SFXManager.SfxManagerInstance.PlayGettingRightLetter(transform.position);
               // break;
            }
        }
    }
}
