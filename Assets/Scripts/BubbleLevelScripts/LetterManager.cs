using System;
using System.Text;
using UnityEngine;


public class LetterManager : MonoBehaviour
{
    private string[] _guessWords = new string[6];
    private string _ChosenWord;
    private Vector3 _spawnPoint;
    [SerializeField] private GameObject _letterPrefab;
    private Sprite[] _spritesArray;
    protected string[] Alphabet;

    private void Awake()
    {
        _guessWords[0] = "Gato";
       _guessWords[1] = "Azul";
       _guessWords[2] = "Tigre";
       _guessWords[3] = "Morango";
       _guessWords[4] = "Banana";
       _guessWords[5] = "Chocolate";
    }

    void Start()
    {
        //x = -7.15 y =-3.44
        _ChosenWord = _guessWords[UnityEngine.Random.Range(0, 5)];
        _spawnPoint = new Vector3(-7.15f, -3.44f);
        Debug.Log(_ChosenWord);
        for (int i = 0; i < _ChosenWord.Length; i++)
        {
            Instantiate(_letterPrefab,  _spawnPoint, Quaternion.identity);
            _spawnPoint += new Vector3(2f, 0, 0f);
        }
        _spritesArray = Resources.LoadAll<Sprite>("Letters");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeAlphabet()
    {
        Alphabet = new string[] 
        {
            "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", 
            "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", 
            "u", "v", "w", "x", "y", "z"
        };
    }
}
