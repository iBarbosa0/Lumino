using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableNote : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int noteValue;
    private Vector3 startLocalPosition;
    private RectTransform parentRectTransform;
    private CanvasGroup canvasGroup;

    private bool isClone = false;

    void Awake()
    {
        parentRectTransform = transform.parent as RectTransform;
        startLocalPosition = transform.localPosition;

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Se não for um clone, cria um clone
        if (!isClone)
        {
            GameObject clone = Instantiate(gameObject, transform.parent);
            DraggableNote cloneScript = clone.GetComponent<DraggableNote>();
            cloneScript.isClone = true;
            cloneScript.noteValue = this.noteValue;
            clone.transform.localPosition = startLocalPosition;
            clone.transform.SetAsLastSibling();  // Garante que fique à frente na hierarquia visual

            // Cancela o arrasto no objeto original
            eventData.pointerDrag = clone;
            clone.GetComponent<CanvasGroup>().blocksRaycasts = false;
            clone.GetComponent<DraggableNote>().OnDrag(eventData);
            return;
        }

        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentRectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 localPoint))
        {
            transform.localPosition = localPoint;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // Se não estiver sobre uma dropzone, volta para a posição original
        if (eventData.pointerEnter == null || !eventData.pointerEnter.CompareTag("DropZone"))
        {
            if (isClone)
            {
                Destroy(gameObject);
            }
            else
            {
                transform.localPosition = startLocalPosition;
            }
        }
    }

    public void ResetPosition()
    {
        transform.localPosition = startLocalPosition;
    }
}