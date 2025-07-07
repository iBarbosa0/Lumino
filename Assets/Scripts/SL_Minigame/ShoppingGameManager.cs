using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class ShoppingGameManager : MonoBehaviour
{
    public static ShoppingGameManager instance;
    [SerializeField] private GameObject[] shoppingItemsSprites =  new GameObject[11];
    [SerializeField] private GameObject[] shoppingItemslist =  new GameObject[11];
    public List<GameObject> availableshoppingItems = new List<GameObject>();
    public List<GameObject> shoppingItems = new List<GameObject>();
    List<int> _itemsalreadyselected = new List<int>();
    private Vector3 _rectTransformPosition =  new Vector3(1572, 540.7663f);
    private Vector3[] initialpositions = new Vector3[11];
    public Text LevelTextUI;
    public Text CartCountTextUI;

    [SerializeField] private GameObject endGamePanel;

    private int level = 1;
    public int correctItemsInCart = 0;

    void Awake()
    {
        instance = this;
    }

    public void UpdateLevelUI()
    {
        LevelTextUI.text = "N�vel " + level;
    }

    public void UpdateCartCountUI()
    {
        CartCountTextUI.text = correctItemsInCart + "/" + shoppingItems.Count;
    }

    public void CheckLevelProgression()
    {
        if (correctItemsInCart >= shoppingItems.Count)
        {
            level++;

            // Limpa objetos do n�vel anterior
            foreach (GameObject obj in shoppingItems)
            {
                if (obj != null)
                {
                    obj.SetActive(false);

                    // Reset da posi��o
                    PickUpObjects pickup = obj.GetComponent<PickUpObjects>();
                    if (pickup != null)
                    {
                        obj.transform.SetParent(pickup.originalParent);
                        obj.GetComponent<RectTransform>().anchoredPosition = pickup.startPosition;
                    }
                }
            }

            for (int i = 0; i < shoppingItemslist.Length; i++)
            {
                shoppingItemsSprites[i].transform.position =  initialpositions[i];
            }

            // Reset dos riscos visuais
            DropZone_Cart dropZone = FindObjectOfType<DropZone_Cart>();
            if (dropZone != null)
            {
                foreach (GameObject risco in dropZone.allRiskObjects)
                {
                    if (risco != null)
                        risco.SetActive(false);
                }
            }

            shoppingItems.Clear();
            _rectTransformPosition = new Vector3(1572, 540.7663f);
            correctItemsInCart = 0;

            if (level > 3)
            {
                Debug.Log("Todos os n�veis conclu�dos!");
                if (endGamePanel != null) endGamePanel.SetActive(true);
                return;
            }

            UpdateLevelUI();
            LevelSelect();
            UpdateCartCountUI();
        }
    }

    void Start()
    {
        for (int i = 0; i < shoppingItemslist.Length; i++)
        {
            initialpositions[i] = shoppingItemsSprites[i].gameObject.transform.position;
            _itemsalreadyselected.Add(i);
        }

        correctItemsInCart = 0;
        LevelSelect();
        UpdateLevelUI();
        UpdateCartCountUI();
    }

    void LevelSelect()
    {
        correctItemsInCart = 0;
        UpdateLevelUI();
        UpdateCartCountUI();

        List<int> selectedItems = new();
        int itemsCount = level == 1 ? 3 : level == 2 ? 5 : 8;

        for (int i = 0; i < itemsCount; i++)
        {
            List<int> availableItems = Enumerable.Range(0, _itemsalreadyselected.Count)
                .Where(num => !selectedItems.Contains(num)).ToList();

            int itemselected = availableItems[Random.Range(0, availableItems.Count)];
            if (!shoppingItems.Contains(shoppingItemslist[itemselected]))
            {
                shoppingItems.Add(shoppingItemslist[itemselected]);
                selectedItems.Add(itemselected);
            }
        }

        setupshopingItems();
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
