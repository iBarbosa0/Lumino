using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ShoppingGameManager : MonoBehaviour
{
    public static ShoppingGameManager instance;
    [SerializeField] private GameObject[] shoppingItemsSprites =  new GameObject[12];
    [SerializeField] private GameObject[] shoppingItemslist =  new GameObject[12];
    List<GameObject> availableshoppingItems = new List<GameObject>();
    List<GameObject> shoppingItems = new List<GameObject>();
    List<int> _itemsalreadyselected = new List<int>();
    private Vector3 _rectTransformPosition =  new Vector3(1572, 540.7663f);
    
    

    private int level = 2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        instance = this;
    }
    
    void Start()
    {
        for (int i = 0; i < shoppingItemslist.Length; i++)
        {
            _itemsalreadyselected.Add(i);
        }
        LevelSelect();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LevelSelect()
    {
        if (level == 1)
        {
            List<int> selectedItems = new();
            for (int i = 0; i < 3; i++)
            { 
                List<int> availableItems = Enumerable.Range(0, _itemsalreadyselected.Count)
                    .Where(num => !selectedItems.Contains(num)).ToList();
                
               int itemselected = availableItems[Random.Range(0, availableItems.Count)];
               if(!shoppingItems.Contains(shoppingItemslist[itemselected]))
               {
                   shoppingItems.Add(shoppingItemslist[itemselected]);
                   selectedItems.Add(itemselected);
               }
            }
            setupshopingItems();
        }
        if (level == 2)
        {
            List<int> selectedItems = new();
            for (int i = 0; i < 5; i++)
            { 
                List<int> availableItems = Enumerable.Range(0, _itemsalreadyselected.Count)
                    .Where(num => !selectedItems.Contains(num)).ToList();
                
                int itemselected = availableItems[Random.Range(0, availableItems.Count)];
                if(!shoppingItems.Contains(shoppingItemslist[itemselected]))
                {
                    shoppingItems.Add(shoppingItemslist[itemselected]);
                    selectedItems.Add(itemselected);
                }
            }
            setupshopingItems();
        }
        if (level == 3)
        {
            List<int> selectedItems = new();
            for (int i = 0; i < 8; i++)
            { 
                List<int> availableItems = Enumerable.Range(0, _itemsalreadyselected.Count)
                    .Where(num => !selectedItems.Contains(num)).ToList();
                
                int itemselected = availableItems[Random.Range(0, availableItems.Count)];
                if(!shoppingItems.Contains(shoppingItemslist[itemselected]))
                {
                    shoppingItems.Add(shoppingItemslist[itemselected]);
                    selectedItems.Add(itemselected);
                }
            }
            setupshopingItems();
        }
    }

    void setupshopingItems()
    {
        if (level == 1)
        {
            for (int i = 0; i < shoppingItems.Count; i++)
            {
                shoppingItems[i].gameObject.SetActive(true);
                shoppingItems[i].GetComponent<RectTransform>().anchoredPosition = _rectTransformPosition;
                _rectTransformPosition += new Vector3(0, -200f, 0);
            }
        }
        
        if (level == 2)
        {
            for (int i = 0; i < 5; i++)
            {
                shoppingItems[i].gameObject.SetActive(true);
                shoppingItems[i].GetComponent<RectTransform>().anchoredPosition = _rectTransformPosition;
                _rectTransformPosition += new Vector3(0, -120f, 0);
            }
        }
        
        if (level == 3)
        {
            for (int i = 0; i < 8; i++)
            {
                shoppingItems[i].gameObject.SetActive(true);
                shoppingItems[i].GetComponent<RectTransform>().anchoredPosition = _rectTransformPosition;
                shoppingItems[i].GetComponent<Transform>().localScale = new Vector3(0.55f, 0.55f, 0.1f);
                _rectTransformPosition += new Vector3(0, -70f, 0);
            }
        }
       
    }
}
