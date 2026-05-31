using UnityEngine;
using UnityEngine.InputSystem;

public class IsometricCamera : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;

    [Header("Zoom")]
    public float zoomSpeed = 2f;
    public float minZoom = 5f;
    public float maxZoom = 20f;

    private Camera cam;
    private float fixedY;

    void Awake()
    {
        cam = GetComponent<Camera>();
        transform.rotation = Quaternion.Euler(45f, 45f, 0f);
        fixedY = transform.position.y;
    }

    private void Start()
    {
        // Grid 20x20, merkez dünya koordinatı (10, 0, 10)
        // Kamera 45/45 açıda, orthographic size 15 → grid tam ortada görünsün
        transform.position = new Vector3(10f, 20f, 10f);
        fixedY = 20f;

        if (cam != null)
            cam.orthographicSize = 15f;
    }

    void Update()
    {
        HandleMovement();
        HandleZoom();
    }

    void HandleMovement()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        float h = 0f;
        float v = 0f;

        if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) h += 1f;
        if (kb.aKey.isPressed || kb.leftArrowKey.isPressed)  h -= 1f;
        if (kb.wKey.isPressed || kb.upArrowKey.isPressed)    v += 1f;
        if (kb.sKey.isPressed || kb.downArrowKey.isPressed)  v -= 1f;

        Vector3 right   = new Vector3(1f, 0f, 0f);
        Vector3 forward = new Vector3(0f, 0f, 1f);

        Vector3 direction = (right * h + forward * v).normalized;
        Vector3 next = transform.position + direction * moveSpeed * Time.deltaTime;
        transform.position = new Vector3(next.x, fixedY, next.z);
    }

    void HandleZoom()
    {
        var mouse = Mouse.current;
        if (mouse == null) return;

        float scroll = mouse.scroll.ReadValue().y;
        if (scroll == 0f) return;

        cam.orthographicSize = Mathf.Clamp(
            cam.orthographicSize - scroll * zoomSpeed * Time.deltaTime,
            minZoom,
            maxZoom
        );
    }
}
