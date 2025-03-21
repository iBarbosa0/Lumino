using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
   private RectTransform _rectTransform;
   private Canvas canvas;
   private Vector2 initialPosition;
   private CanvasGroup _canvasGroup;

   private void Awake()
   {
      _rectTransform = GetComponent<RectTransform>();
      canvas = GetComponentInParent<Canvas>();
      _canvasGroup  = GetComponent<CanvasGroup>();
   }

   public void OnBeginDrag(PointerEventData eventData)
   {
      initialPosition = _rectTransform.anchoredPosition;
      Debug.Log("Drag started!");
      _canvasGroup.alpha = 0.6f;
      _canvasGroup.blocksRaycasts = false;
   }

   public void OnDrag(PointerEventData eventData)
   {
      // Convert screen coordinates to anchored position
      Vector2 localPoint;
      RectTransformUtility.ScreenPointToLocalPointInRectangle(
         _rectTransform.parent as RectTransform,
         eventData.position,
         eventData.pressEventCamera,
         out localPoint
      );
      _rectTransform.anchoredPosition = localPoint;
   }

   public void OnEndDrag(PointerEventData eventData)
   {
      Debug.Log("Drag ended!");
      _canvasGroup.alpha = 1f;
      _canvasGroup.blocksRaycasts = true;

   }

   public void OnPointerDown(PointerEventData eventData)
   {
      Debug.Log("OnPointerDown");
   }

   public void OnDrop(PointerEventData eventData)
   {
     // Debug.Log("OnDrop");
   }
}
