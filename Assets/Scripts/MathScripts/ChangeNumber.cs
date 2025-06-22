using System;
using UnityEngine;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine.UI;
using System.Collections;

public class ChangeNumber : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject numberPrefab;
    SpriteRenderer _numberRenderer;
    Transform _numbertransform;
    private Vector3 _initialposition;
    private int _number = 0;
    public  Button _previousNumber;
    public  Button _nextNumber;
    public  Button _CheckButton;
    private int _timer = 1;

    
  
    void Start()
    {
        _previousNumber.onClick.RemoveAllListeners();
        _previousNumber.onClick.AddListener(PreviousNumber);
        _nextNumber.onClick.RemoveAllListeners();
        _nextNumber.onClick.AddListener(NextNumber);
        _CheckButton.onClick.RemoveAllListeners();
        _CheckButton.onClick.AddListener(checknumber);
       _numberRenderer = numberPrefab.GetComponent<SpriteRenderer>();
       _numbertransform = numberPrefab.GetComponent<Transform>();
       _initialposition = _numberRenderer.transform.position;
       Debug.Log(" Start Initial Position "+ _initialposition);
       _numberRenderer.sprite = MathGameManager.Instance.NumberSpritesArray[0];
    }

    private void FixedUpdate()
    {

       // if ()
       // {
            
       // }
        
    }

    public void PreviousNumber()
    {
        if (_number-1<0)
        {
            _number = 9;
        }
        else
        {
            _number--;
        }
        _numberRenderer.sprite = MathGameManager.Instance.NumberSpritesArray[_number];
       // Debug.Log(_number);
    }
    
    public void NextNumber()
    {
        if (_number+1>9)
        {
            _number = 0;
        }
        else
        {
            _number++;
        }
       
        _numberRenderer.sprite = MathGameManager.Instance.NumberSpritesArray[_number];
        //Debug.Log(_number);
    }

    void checknumber()
    {
        if (MathGameManager.Instance.sumorsubtraction == true)
        {
            if (MathGameManager.Instance.integer1+MathGameManager.Instance.integer2 == _number)
            {
                MathGameManager.Instance.DisableUI();
                return;
            }
            else
            {
               // Debug.Log("incorrectnumber");
                TriggerShake();
            }
        }
        else
        {
            if (MathGameManager.Instance.integer1-MathGameManager.Instance.integer2 == _number)
            {
                MathGameManager.Instance.DisableUI();
                return;
            }
            else
            {
                TriggerShake();
            }
        }
    }

    public void TriggerShake()
    {
        StopAllCoroutines(); // Reset any ongoing shakes
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        Debug.Log(" Shake Initial Position "+ _initialposition);
        Vector3 changeposition = _initialposition;
        float elapsed = 0f;

        while (elapsed < _timer)
        {
            float x = Mathf.Sin(elapsed * 16) * 0.5f;
            _numbertransform.position = changeposition + new Vector3(x, 0, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        _numbertransform.position = _initialposition;
    }
    
}
