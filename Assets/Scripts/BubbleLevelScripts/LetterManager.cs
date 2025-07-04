using System;
using System.Text;
using UnityEngine;
using System.Collections.Generic;

public class LetterManager : MonoBehaviour
{
    public static LetterManager  LetterManagerInstance; //Static Variable to acess it globally 
    private string[] _guessWords = new string[9]; //GuessWordList
    public string ChosenWord { get; private set; } //chosen word from guessworldList
    private Vector3 _spawnPoint; //Spawnpoint for spawing the cases for the letters
    [SerializeField] private GameObject _letterPrefab; // The Case letters prefab txo make it spawn dynamically 
    public GameObject[] BubbleLetterBox { get; private set; } // Game object array to store the CaseLetters prefab that will be Instantiated 
    public Sprite[] SpritesArray { get; private set; } //Array to store all the letters of the alphabets sprites
    public char[] Alphabet { get; private set; } //char string to store all the letters of the alphabet
    public int[] AlphabetPosition { get; private set; }
    public Queue<int> QueueAlphabetPosition  = new Queue<int>();
    
    //load Sprites
    [SerializeField] private GameObject keywordPrefab;
    [SerializeField] private Sprite lua;
    [SerializeField] private Sprite banana;
    [SerializeField] private Sprite carro;
    [SerializeField] private Sprite chocolate;
    [SerializeField] private Sprite gato;
    [SerializeField] private Sprite livro;
    [SerializeField] private Sprite pizza;
    [SerializeField] private Sprite sol;
    [SerializeField] private Sprite sapo;

    //Dictionary to match guess word to the respective sprite
    private Dictionary<string, Sprite> _guessWordsDictionary = new Dictionary<string, Sprite>();

    private void Awake()
    {
        if (LetterManagerInstance == null)
        {
            LetterManagerInstance = this;
        }
        _guessWords[0] = "gato";
        _guessWords[1] = "lua";
        _guessWords[2] = "carro";
        _guessWords[3] = "sol";
        _guessWords[4] = "sapo";
        _guessWords[5] = "pizza";
        _guessWords[6] = "livro";
        _guessWords[7] = "banana";
      //  _guessWords[8] = "chocolate";
        _guessWordsDictionary.Add(_guessWords[0], gato);
        _guessWordsDictionary.Add(_guessWords[1], lua);
        _guessWordsDictionary.Add(_guessWords[2], carro);
        _guessWordsDictionary.Add(_guessWords[3], sol);
        _guessWordsDictionary.Add(_guessWords[4], sapo);
        _guessWordsDictionary.Add(_guessWords[5], pizza);
        _guessWordsDictionary.Add(_guessWords[6], livro);
        _guessWordsDictionary.Add(_guessWords[7], banana);
      //  _guessWordsDictionary.Add(_guessWords[8], chocolate);

    }

    void Start()
    {
        InitializeAlphabet();
        ChosenWord = _guessWords[UnityEngine.Random.Range(0, _guessWords.Length)];
        GameObject Keyword = Instantiate(keywordPrefab, new Vector3(7, -3.44f), Quaternion.identity);
        Keyword.GetComponent<SpriteRenderer>().sprite = _guessWordsDictionary[ChosenWord];
        AlphabetPosition = new int[ChosenWord.Length];
        BubbleLetterBox =  new GameObject[ChosenWord.Length];
        _spawnPoint = new Vector3(-8f, -4.44f);
        Debug.Log(ChosenWord); 
        float distanceBetweenLetters = 10/ChosenWord.Length +1f;

        for (int i = 0; i < ChosenWord.Length; i++)
        { 
            //Instantiate(_letterPrefab,  _spawnPoint, Quaternion.identity);
            BubbleLetterBox[i] = Instantiate(_letterPrefab,  _spawnPoint, Quaternion.identity);
            BubbleLetterBox[i].AddComponent<BubbleLetterBox>();
            BubbleLetterBox[i].transform.localScale = new Vector3((1f/ChosenWord.Length)+0.5f, 1f/ChosenWord.Length+0.5f);
            _spawnPoint += new Vector3(distanceBetweenLetters, 0, 0f);
        }
        SpritesArray = Resources.LoadAll<Sprite>("Letters");
        int j = 0;
        foreach (char c in ChosenWord.ToCharArray())
        {
            for (int i = 0; i < 26; i++)
            {
                if (c == Alphabet[i] )
                {
                    QueueAlphabetPosition.Enqueue(i);
                    AlphabetPosition[j] = i;
                    BubbleLetterBox[j].GetComponent<BubbleLetterBox>().Initialize(i);
                    j++;
                }
            }
        }

        for (int i = 0; i < AlphabetPosition.Length; i++)
        {
            Debug.Log(AlphabetPosition[i]);
        }
    }
    private void InitializeAlphabet()
    {
        Debug.Log(("Initializing alphabet"));
        Alphabet = new char[] 
        {
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 
            'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 
            'u', 'v', 'w', 'x', 'y', 'z'
        };
    }
}
