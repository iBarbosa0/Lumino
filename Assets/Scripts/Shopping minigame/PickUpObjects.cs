using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PickUpObjects : MonoBehaviour,IDragHandler, IPointerDownHandler,IDropHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector3 startPosition;

    void Start()
    {
        // Get the RectTransform and Canvas components
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.anchoredPosition;
        canvas = GetComponentInParent<Canvas>();
    }

    // Called when the mouse button is pressed down
    public void OnPointerDown(PointerEventData eventData)
    {
        // Optional: Bring the UI element to the front (if multiple overlapping elements)
        rectTransform.SetAsLastSibling();
        Debug.Log("OnPointerDown");
    }

    // Called when dragging the UI element
    public void OnDrag(PointerEventData eventData)
    {
        // Update the position of the UI element based on mouse movement
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
    public void OnDrop(PointerEventData eventData)
    {
        rectTransform.anchoredPosition = startPosition;
    }
}
