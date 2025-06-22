using System;
using TMPro;
using UnityEngine;

public class MathGameManager : MonoBehaviour
{
    
    public static MathGameManager Instance;
    public Sprite[] NumberSpritesArray { get; private set; } 
    public Sprite[] ArithmeticSymbolsArray { get; private set; } 

    [SerializeField] private GameObject number1 ;
    [SerializeField] private GameObject number2 ;
    [SerializeField] private GameObject symbol ;
    //[SerializeField] private GameObject Equal ;
    public int integer1{ get; private set; }
    public int integer2{ get; private set; }

    public bool sumorsubtraction{ get; private set; }
    private DrawOnScreen _drawOnScreen;
    private int _pontos = 0;

    //UIElEMENTS
    [SerializeField] private GameObject previousButton;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject rightAnswerText;
    [SerializeField] private GameObject quad;
    [SerializeField] private GameObject reDoDrawingButton;
    [SerializeField] private GameObject checkDrawingButton;
    [SerializeField] private GameObject checkanswerbutton;
    [SerializeField] private TMP_Text pontosText;


    


    
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        NumberSpritesArray = Resources.LoadAll<Sprite>("Numbers");
        ArithmeticSymbolsArray = Resources.LoadAll<Sprite>("Symbols");
    }
    void Start()
    {
        _drawOnScreen = quad.GetComponent<DrawOnScreen>();
        //NumberSpritesArray = Resources.LoadAll<Sprite>("Numbers");
        //ArithmeticSymbolsArray = Resources.LoadAll<Sprite>("Symbols");
        Debug.Log(NumberSpritesArray.Length);
        for (int i = 0; i < 10; i++)
        {
            Debug.Log(NumberSpritesArray[i].name); 
        }
        //Equal.GetComponent<SpriteRenderer>().sprite = ArithmeticSymbolsArray[2];
        ChoseArithmetic();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    private void ChoseArithmetic()
    {
        Score();
        int random = UnityEngine.Random.Range(0, 2);
        Debug.Log(random);
        if (random == 0)
        {
             integer1 = UnityEngine.Random.Range(0, 10);
             integer2 = UnityEngine.Random.Range(0, 10);
            while (integer1 + integer2 >= 10)
            {
                integer1 = UnityEngine.Random.Range(0, 10);
                integer2 = UnityEngine.Random.Range(0, 10);
            }
            symbol.GetComponent<SpriteRenderer>().sprite = ArithmeticSymbolsArray[0];
            sumorsubtraction = true;
        }
        else
        {
            integer1 = UnityEngine.Random.Range(0, 10);
            integer2 = UnityEngine.Random.Range(0, 10);
            while ( integer1 - integer2 < 0)
            {
                integer1 = UnityEngine.Random.Range(0, 10);
                integer2 = UnityEngine.Random.Range(0, 10);
            }
            symbol.GetComponent<SpriteRenderer>().sprite = ArithmeticSymbolsArray[1];
            sumorsubtraction = false;
        }
        
        number1.GetComponent<SpriteRenderer>().sprite = NumberSpritesArray[integer1];
        number2.GetComponent<SpriteRenderer>().sprite = NumberSpritesArray[integer2];
    }
    
    
    public void DisableUI()
    {
        StartCoroutine(_drawOnScreen.paintboardblackEnumerator());
        previousButton.SetActive(false);
        nextButton.SetActive(false);
        rightAnswerText.SetActive(true);
        checkanswerbutton.SetActive(false);
        quad.SetActive(true);
        reDoDrawingButton.SetActive(true);
        checkDrawingButton.SetActive(true);
        _drawOnScreen.PaintBoardBlack();
        _drawOnScreen.PaintBoardBlack();
        _drawOnScreen.PaintBoardBlack();

        
    }
    
    public void EnableUI()
    {
        previousButton.SetActive(true);
        nextButton.SetActive(true);
        rightAnswerText.SetActive(false);
        checkanswerbutton.SetActive(true);
        quad.SetActive(false);
        reDoDrawingButton.SetActive(false);
        checkDrawingButton.SetActive(false);
        
    }

    public void checkifimageiscorrect(int numberdrawn)
    {
       /* if (sumorsubtraction==true)
        {
            if (integer1+integer2 == numberdrawn)
            {
                _pontos++;
                ChoseArithmetic();
                EnableUI();
                _drawOnScreen.PaintBoardBlack();
            }
            else
            {
                _drawOnScreen.PaintBoardBlack();
            }
        }
        else
        {
            if (integer1-integer2 == numberdrawn)
            {
                _pontos++;
                ChoseArithmetic();
                EnableUI();
                _drawOnScreen.PaintBoardBlack();
            }
            else
            {
                _drawOnScreen.PaintBoardBlack();
            }
        }
        */
       _pontos++;
       ChoseArithmetic();
       EnableUI();
       _drawOnScreen.PaintBoardBlack();
        
    }

    private void Score()
    {
        pontosText.text = "Pontos: " + _pontos.ToString();
    }
   
}
