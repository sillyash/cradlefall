using UnityEngine;
using Unity.Netcode;

public class MouseControl : NetworkBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float sensibility = 200f;

    private float xRotation = 0f; // vertical
    private float yRotation = 0f; // horizontal

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (!IsOwner) return;

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
