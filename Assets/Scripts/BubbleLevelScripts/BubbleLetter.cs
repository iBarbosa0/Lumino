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
    private int positioncounter = 0;
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

        if (LetterManager.LetterManagerInstance.QueueAlphabetPosition.Count == 0)
        {
           // return;
        }
        if (LetterPosition == LetterManager.LetterManagerInstance.QueueAlphabetPosition.Peek())
        {
            SpriteRenderer _spriteRenderer = LetterManager.LetterManagerInstance.BubbleLetterBox[LetterManager.LetterManagerInstance.ChosenWord.Length-LetterManager.LetterManagerInstance.QueueAlphabetPosition.Count].transform.Find("Letter").GetComponent<SpriteRenderer>();// Get the spriterenderer of the case letter that i want to change 
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 1f); //access the sprite renderer of the letter i want to change and change opacity to 1.0
            LetterManager.LetterManagerInstance.QueueAlphabetPosition.Dequeue(); //POP the correct letter of the queu list;
            SFXManager.SfxManagerInstance.PlayGettingRightLetter(transform.position); //play audio of getting the right letter
        }
    }
}
