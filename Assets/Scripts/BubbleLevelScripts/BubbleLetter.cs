using UnityEngine;

public class BubbleLetter : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Sprite[] _spritesArray;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("awsfnlaslkn");
         _spriteRenderer = GetComponent<SpriteRenderer>();
         _spritesArray = Resources.LoadAll<Sprite>("Letters");
         _spriteRenderer.sprite = _spritesArray[UnityEngine.Random.Range(0, 25)];
         Debug.Log("spritearray " + _spritesArray.Length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
