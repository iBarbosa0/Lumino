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
        Debug.Log("Sprite Renderer" + _spriteRenderer.name);
        //Debug.Log("letterposition" + letterPosition);
        _spriteRenderer.sprite = LetterManager.LetterManagerInstance.SpritesArray[letterPosition];
        _spriteRenderer.color= new Color(_spriteRenderer.color.r, _spriteRenderer.color.g,_spriteRenderer.color.b, 0.5f);
    }
 
}
