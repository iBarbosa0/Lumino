using System;
using System.Text;
using UnityEngine;
using System.Collections.Generic;

public class LetterManager : MonoBehaviour
{
    public static LetterManager  LetterManagerInstance;
    private string[] _guessWords = new string[6];
    public string _ChosenWord { get; private set; }
    private Vector3 _spawnPoint;
    [SerializeField] private GameObject _letterPrefab;
    public Sprite[] SpritesArray { get; private set; }
    public char[] Alphabet { get; private set; }
    public int[] AlphabetPosition { get; private set; }


    private void Awake()
    {
        if (LetterManagerInstance == null)
        {
            LetterManagerInstance = this;
        }
        _guessWords[0] = "gato";
       _guessWords[1] = "copo";
       _guessWords[2] = "tigre";
       _guessWords[3] = "morango";
       _guessWords[4] = "banana";
       _guessWords[5] = "chocolate";
    }

    void Start()
    {
        InitializeAlphabet();
        _ChosenWord = _guessWords[UnityEngine.Random.Range(0, 5)];
        AlphabetPosition = new int[_ChosenWord.Length];
        _spawnPoint = new Vector3(-7.15f, -3.44f);
        Debug.Log(_ChosenWord);
        for (int i = 0; i < _ChosenWord.Length; i++)
        {
            Instantiate(_letterPrefab,  _spawnPoint, Quaternion.identity);
            _spawnPoint += new Vector3(2f, 0, 0f);
        }
        SpritesArray = Resources.LoadAll<Sprite>("Letters");
        int j = 0;
        foreach (char c in _ChosenWord.ToCharArray())
        {
            for (int i = 0; i < 25; i++)
            {
                if (c == Alphabet[i] )
                {
                    AlphabetPosition[j] = i;
                    j++;
                }
            }
        }

        for (int i = 0; i < AlphabetPosition.Length; i++)
        {
            Debug.Log(AlphabetPosition[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
