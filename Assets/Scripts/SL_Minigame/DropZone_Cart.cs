using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static ShoppingItemList;

public class DropZone_Cart : MonoBehaviour, IDropHandler
{
    public List<GameObject> allShoppingListWordObjects; // Lista de palavras da lista de compras (ex: banana, bife, etc.)
    public List<GameObject> allRiskObjects; // Lista dos riscos visuais (na mesma ordem)
    public AudioClip riskSound;
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        if (dropped == null) return;

        ShoppingItemList item = dropped.GetComponent<ShoppingItemList>();
        if (item == null)
        {
            return;
        }

        string itemName = item.itemName.ToLower(); // Nome do item arrastado (e.g., "banana")

        bool itemFoundInCurrentLevelList = false;
        GameObject matchedWordGameObject = null; // Guarda o GameObject da palavra (e.g., w_banana) se encontrado na lista ATIVA

        // Obtém a lista de GameObjects de palavras que estão ATIVAS para o nível atual do GameManager
        List<GameObject> currentActiveShoppingListWords = ShoppingGameManager.instance.shoppingItems;

        // Itera SOMENTE sobre os GameObjects que estão ATIVOS na lista de compras do nível
        foreach (GameObject activeWordObjectInList in currentActiveShoppingListWords)
        {
            if (activeWordObjectInList == null)
            {
                continue; // Pula para o próximo
            }

            string wordNameFromActiveList = activeWordObjectInList.name.ToLower(); // Nome do GameObject da palavra ativa (e.g., "w_leite")

            if (wordNameFromActiveList.StartsWith("w_"))
            {
                //Debug.Log($"     wordNameFromActiveList.Substring(2) (parte do nome para comparação): '{wordNameFromActiveList.Substring(2)}'");
                //Debug.Log($"     Comparação final (wordNameFromActiveList.Substring(2) == itemName): {wordNameFromActiveList.Substring(2) == itemName}");
            }

            // Apenas verifica se começa com "w_" e se o nome após "w_" é igual ao nome do item arrastado
            if (wordNameFromActiveList.StartsWith("w_") && wordNameFromActiveList.Substring(2) == itemName)
            {
                itemFoundInCurrentLevelList = true;
                matchedWordGameObject = activeWordObjectInList; // Armazena a referência para a palavra encontrada
                break; // Item encontrado na lista ativa, não precisa de continuar a procurar
            }
        }

        // Se o item foi encontrado na lista ATIVA do nível atual
        if (itemFoundInCurrentLevelList)
        {
            dropped.transform.SetParent(transform); // Faz o item arrastado ser filho do carrinho

            // Encontrar o risco correspondente e ativa-o
            int indexOfMatchedWord = -1;
            for (int i = 0; i < allShoppingListWordObjects.Count; i++)
            {
                // Precisamos encontrar o índice do matchedWordGameObject na lista completa de palavras
                if (allShoppingListWordObjects[i] == matchedWordGameObject)
                {
                    indexOfMatchedWord = i;
                    break;
                }
            }

            if (indexOfMatchedWord != -1 && allRiskObjects.Count > indexOfMatchedWord && allRiskObjects[indexOfMatchedWord] != null)
            {
                AudioManager.instance.PlayRiskSound();
                allRiskObjects[indexOfMatchedWord].SetActive(true);
                // Posiciona o risco na mesma posição da palavra correspondente
                allRiskObjects[indexOfMatchedWord].GetComponent<RectTransform>().position =
                    matchedWordGameObject.GetComponent<RectTransform>().position;
            }
            else
            {
                //Debug.LogWarning($"Não foi possível encontrar ou ativar o risco para '{itemName}'. Verifique as listas 'allShoppingListWordObjects' e 'allRiskObjects' no Inspector do DropZone_Cart.");
            }

            // Notifica o PickUpObjects para que o item permaneça no carrinho
            PickUpObjects pickupScript = dropped.GetComponent<PickUpObjects>();
            if (pickupScript != null)
            {
                ShoppingGameManager.instance.correctItemsInCart++;
                ShoppingGameManager.instance.UpdateCartCountUI();
                ShoppingGameManager.instance.CheckLevelProgression();

                pickupScript.MarkAsDroppedInZone();
            }
        }
        else // Se o item NÃO foi encontrado na lista ATIVA do nível atual
        {
            Debug.Log($"Item '{itemName}' é INCORRETO para o nível atual. Não será marcado como dropado na zona. Deverá voltar à origem.");
            // O PickUpObjects.OnEndDrag vai lidar com o retorno à posição inicial automaticamente.
            // Não fazer dropped.transform.SetParent(transform) aqui.
        }
    }
}
