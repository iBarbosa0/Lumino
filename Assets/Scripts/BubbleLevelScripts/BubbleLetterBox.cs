using System;
using UnityEngine;

public class BubbleLetterBox : MonoBehaviour
{
    public int letterPosition;
    private SpriteRenderer _spriteRenderer;
    public void Initialize(int LetterPosition)
    {
        Transform childTransform = transform.Find("Letter");
        _spriteRenderer = childTransform.GetComponent<SpriteRenderer>();
        letterPosition = LetterPosition;

        if (LetterManager.LetterManagerInstance.Level < 2)
        {
            _spriteRenderer.sprite = LetterManager.LetterManagerInstance.SpritesArray[letterPosition];
            _spriteRenderer.color= new Color(_spriteRenderer.color.r, _spriteRenderer.color.g,_spriteRenderer.color.b, 0.5f);
            return;
        }
        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            return;
        }
            _spriteRenderer.sprite = LetterManager.LetterManagerInstance.SpritesArray[letterPosition];
            _spriteRenderer.color= new Color(_spriteRenderer.color.r, _spriteRenderer.color.g,_spriteRenderer.color.b, 0.5f);
    }
 
}
