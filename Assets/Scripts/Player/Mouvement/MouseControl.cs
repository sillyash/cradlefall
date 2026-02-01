using DG.Tweening;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;

public class MouseControl : NetworkBehaviour
{
    [SerializeField] private Transform chest;
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
        xRotation += mouseY;         
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Prevent vertical over-rotation


        float yRotationSigned = (yRotation > 180) ? yRotation - 360 : yRotation;

        //
        //if (math.abs(yRotation + yRotationSigned) >= 90f)
        //{
        // Apply horizontal rotation to player
        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
        // Apply vertical rotation to chest (camera)
        chest.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        //}
        //else
        //{
        //    // Apply full rotation to chest (camera)
        //    chest.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        //}
    }
}
