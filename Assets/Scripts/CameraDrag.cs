using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    private Vector3 _dragOrigin; // Where the drag started
    private bool _isDragging = false;
    [SerializeField] private float dragSpeed = 1f; // sensitivity
    [SerializeField] private Vector2 minBounds; // minimum x, y camera position
    [SerializeField] private Vector2 maxBounds; // maximum x, y camera position
    void Update()
    {
        // start dragging
        if (Input.GetMouseButtonDown(0))
        {
            _dragOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _isDragging = true;
            Debug.Log("DragOrigin Point" +_dragOrigin);
        }

        // stop dragging
        if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
        }

        // move camera while dragging
        if (_isDragging)
        {
            Vector3 currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 move = _dragOrigin - currentPos;
            Vector3 newPosition = transform.position + move * dragSpeed;

            // Clamp to make sure the camera doesn't go beyond the map
            newPosition.x = Mathf.Clamp(newPosition.x, minBounds.x, maxBounds.x);
            newPosition.y = Mathf.Clamp(newPosition.y, minBounds.y, maxBounds.y);
            //newPosition.z = transform.position.z; // Keep z unchanged (e.g., -10)

            transform.position = newPosition;
        }
    }
}
