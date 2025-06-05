using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public NoteMinigame noteMinigame;

    public void OnDrop(PointerEventData eventData)
    {
        DraggableNote note = eventData.pointerDrag?.GetComponent<DraggableNote>();
        if (note != null)
        {
            noteMinigame.ReceberNota(note.noteValue);
            Destroy(note.gameObject); // Remove a nota depois de largar
        }
    }
}
