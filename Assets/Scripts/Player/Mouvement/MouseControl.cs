using UnityEngine;

public class MouseControl : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float sensibility = 200f;

    private float xRotation = 0f; // vertical
    private float yRotation = 0f; // horizontal

    void Start()
    {
        mainCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleMouseLook();
    }

    void HandleMouseLook()
    {
        // Get mouse input delta (movement, not cursor position)
        float mouseX = Input.GetAxis("Mouse X") * sensibility * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensibility * Time.deltaTime;

        // Accumulate rotations
        yRotation += mouseX;
        xRotation -= mouseY;         
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Prevent vertical over-rotation

        // Apply horizontal rotation to player
        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

        // Apply vertical rotation to camera
        mainCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
