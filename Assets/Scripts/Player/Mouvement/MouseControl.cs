using DG.Tweening;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class MouseControl : NetworkBehaviour
{
    [SerializeField] private Transform chest;
    [SerializeField] private float sensibility = 200f;

    private float xRotation = 0f; // vertical
    private float yChestRotation = 0f;
    private float yRotation = 0f; // horizontal

    public NetworkVariable<Quaternion> chestNetwork = new NetworkVariable<Quaternion>(Quaternion.Euler(0f,0f,0f), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public Quaternion chestRotation
    {
        get => chestNetwork.Value;
    }

    public override void OnNetworkSpawn()
    {    
        if (!IsOwner) return;

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        chest.localRotation = chestRotation;

        if (!IsOwner) return;

        HandleMouseLook();
    }

    void HandleMouseLook()
    {
        // Get mouse input delta (movement, not cursor position)
        float mouseX = Input.GetAxis("Mouse X") * sensibility * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensibility * Time.deltaTime;

        // Accumulate rotations
        xRotation += mouseY;
        yRotation += mouseX;

        // Get chest rotation (camera)
        yChestRotation += mouseX;
        yChestRotation = Mathf.Clamp(yChestRotation, -90f, 90f); // Clamp it to max bone stretch
        
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Prevent vertical over-rotation


        if (math.abs(yChestRotation) == 90f)
        {
            // Apply non-chest rotation to player
            transform.rotation = Quaternion.Euler(0f, yRotation - yChestRotation, 0f);
        }

        // Apply remaining rotation to chest (camera)
        chestNetwork.Value = Quaternion.Euler(xRotation, yChestRotation, 0f);
    }
}
