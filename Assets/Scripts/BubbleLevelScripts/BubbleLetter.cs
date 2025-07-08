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
    [SerializeField] private GameObject splashAnimationPrefab;
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
       
    }

    public void destroyBubble()
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
            Destroy(gameObject);
        }
        else
        {
            GetComponent<Rigidbody2D>().mass = 1;
            GetComponent<Rigidbody2D>().gravityScale = 1;
            gameObject.AddComponent<BoxCollider2D>().isTrigger = true;
            Destroy(gameObject, 3f);
        }
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            GameObject splash = Instantiate(splashAnimationPrefab, transform.position, Quaternion.identity); ;
            Destroy(gameObject);
        }
      
    }
}
