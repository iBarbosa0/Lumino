using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PickUpObjects : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector3 startPosition;
    private CanvasGroup canvasGroup;

    void Start()
    {
        // Get the RectTransform and Canvas components
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.anchoredPosition;
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
    }

    // Called when dragging the UI element
    public void OnDrag(PointerEventData eventData)
    {
        // Update the position of the UI element based on mouse movement
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!eventData.pointerEnter || !eventData.pointerEnter.GetComponent<DropZone>())
        {
            rectTransform.anchoredPosition = startPosition; // Volta só se não for drop válido
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (canvasGroup != null)
            canvasGroup.blocksRaycasts = true;
    }
}
