using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class BubbleLetter : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Sprite[] _spritesArray;
    public bool wasItPoped = true;
    private int _letterPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            _letterPosition =  LetterManager.LetterManagerInstance.AlphabetPosition[UnityEngine.Random.Range(0, LetterManager.LetterManagerInstance.AlphabetPosition.Length)];
        }
        else
        {
            _letterPosition = UnityEngine.Random.Range(0, 25);
        }
         
         _spriteRenderer = GetComponent<SpriteRenderer>();
         _spriteRenderer.sprite = LetterManager.LetterManagerInstance.SpritesArray[_letterPosition];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        if (wasItPoped == false)
        {
            return;
        }

        if (LetterManager.LetterManagerInstance.QueueAlphabetPosition.Count == 0)
        {
           // return;
        }
        if (_letterPosition == LetterManager.LetterManagerInstance.QueueAlphabetPosition.Peek())
        {
            SpriteRenderer _spriteRenderer = LetterManager.LetterManagerInstance.BubbleLetterBox[LetterManager.LetterManagerInstance.ChosenWord.Length-LetterManager.LetterManagerInstance.QueueAlphabetPosition.Count].transform.Find("Letter").GetComponent<SpriteRenderer>();// Get the spriterenderer of the case letter that i want to change
            _spriteRenderer.sprite = LetterManager.LetterManagerInstance.SpritesArray[_letterPosition];
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 1f); //access the sprite renderer of the letter i want to change and change opacity to 1.0
            LetterManager.LetterManagerInstance.QueueAlphabetPosition.Dequeue(); //POP the correct letter of the queu list;
            SFXManager.SfxManagerInstance.PlayGettingRightLetter(transform.position); //play audio of getting the right letter
            LetterManager.LetterManagerInstance.CheckIfBeatTheLevel();
        }
    }
}
