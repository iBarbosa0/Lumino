using System;
using UnityEngine;

public class BubbleLetterBox : MonoBehaviour
{
    public int letterPosition;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
       // _spriteRenderer = transform.Find("Letter").GetComponent<SpriteRenderer>();
    }

    public void Initialize(int LetterPosition)
    {
        letterPosition = LetterPosition;
       // _spriteRenderer.sprite = LetterManager.LetterManagerInstance.SpritesArray[letterPosition];
        Debug.Log(_spriteRenderer);
        //transform.Find("Letter").GetComponent<SpriteRenderer>().color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g,_spriteRenderer.color.b, 0.5f);
    }
 
}
