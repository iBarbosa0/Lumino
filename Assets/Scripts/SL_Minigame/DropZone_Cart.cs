using UnityEngine;
using UnityEngine.EventSystems;
using static ShoppingItemList;

public class DropZone_Cart : MonoBehaviour, IDropHandler
{
    public GameObject riskBanana;
    public GameObject riskBife;
    public GameObject riskIogurte;

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("DROPPED: " + eventData.pointerDrag.name);

        GameObject dropped = eventData.pointerDrag;
        if (dropped == null) return;

        ShoppingItemList item = dropped.GetComponent<ShoppingItemList>();
        if (item == null) return;

        string itemName = item.itemName.ToLower();

        // Verifica se é um dos itens da lista atual
        switch (itemName)
        {
            case "banana":
                riskBanana.SetActive(true);
                break;
            case "bife":
                riskBife.SetActive(true);
                break;
            case "iogurte":
                riskIogurte.SetActive(true);
                break;
        }

        // Fixa o item no carrinho (pode melhorar depois)
        dropped.transform.SetParent(transform);
        dropped.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }
}