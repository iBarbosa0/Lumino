using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PickUpObjects : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    public Vector3 startPosition;
    public Transform originalParent;  // Novo: guarda o pai original
    private CanvasGroup canvasGroup;

    private bool isDroppedInZone = false; // Controle pra saber se foi dropado na zona

    void Start()
    {
        // Get the RectTransform and Canvas components
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.anchoredPosition;
        originalParent = transform.parent;  // Guarda o pai original
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            Debug.LogWarning("CanvasGroup não encontrado em " + gameObject.name);
        }
    }

    // Called when the mouse button is pressed down
    public void OnPointerDown(PointerEventData eventData)
    {
        // Optional: Bring the UI element to the front (if multiple overlapping elements)
        rectTransform.SetAsLastSibling();
        Debug.Log("OnPointerDown");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (canvasGroup != null)
            canvasGroup.blocksRaycasts = false;

        isDroppedInZone = false; // Reseta ao começar arrastar
    }

    // Called when dragging the UI element
    public void OnDrag(PointerEventData eventData)
    {
        // Update the position of the UI element based on mouse movement
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (canvasGroup != null)
            canvasGroup.blocksRaycasts = true;

        if (!isDroppedInZone)
        {
            transform.SetParent(originalParent);             // Volta para o pai original e reposiciona
            rectTransform.anchoredPosition = startPosition;             // Não foi dropado em nenhuma DropZone — volta pra posição original
        }
    }

    // Método chamado pela DropZone para indicar que o item foi aceito
    public void MarkAsDroppedInZone()
    {
        isDroppedInZone = true;
    }
}
