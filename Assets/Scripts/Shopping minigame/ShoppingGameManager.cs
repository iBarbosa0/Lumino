using UnityEngine;
using System.Collections.Generic;

public class ShoppingGameManager : MonoBehaviour
{
    public static ShoppingGameManager instance;
    [SerializeField] private GameObject[] shoppingItemsSprites =  new GameObject[12];
    [SerializeField] private GameObject[] shoppingItemslist =  new GameObject[12]; 
    
    private GameObject[] shoppingItems;

    private int level = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        instance = this;
    }
    
    void Start()
    {
   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LevelSelect()
    {
        if (level == 1)
        {
            for (int i = 0; i < 3; i++)
            {
                
            }
        }
        if (level == 2)
        {
            
        }
        if (level == 3)
        {
            
        }
        if (level == 4)
        {
            
        }
    }
}
