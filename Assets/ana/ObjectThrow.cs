using UnityEngine;

public class ObjectThrow : MonoBehaviour
{
    private Vector3 offset;
    private Vector3 lastMousePosition;
    private bool isDragging = false;
    private Rigidbody2D rb;
    private string animationName;
    private UIManager uiManager;

    public void Init(string animName, UIManager manager)
    {
        animationName = animName;
        uiManager = manager;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0;
    }

    void OnMouseDown()
    {
        offset = transform.position - GetMouseWorldPosition();
        isDragging = true;
    }

    void OnMouseDrag()
    {
        if (!isDragging) return;
        Vector3 newPos = GetMouseWorldPosition() + offset;
        lastMousePosition = transform.position;
        transform.position = newPos;
    }

    void OnMouseUp()
    {
        isDragging = false;

        Vector2 throwDirection = (transform.position - lastMousePosition).normalized;
        float throwForce = 500f;

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0;
        rb.AddForce(throwDirection * throwForce);

        uiManager.HandleAfterThrow(animationName);
        Destroy(gameObject, 0.2f);
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;  // Distance from camera
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
