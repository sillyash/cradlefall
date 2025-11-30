using UnityEngine;

public class MouseControl : MonoBehaviour
{
    [SerializeField] private BaseWeapon weapon;

    public Transform cameraTransform;
    public float sensibility = 200f;

    float xRotation = 0f; // vertical
    float yRotation = 0f; // horizontal

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensibility * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensibility * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;         
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limite pour empéche de faire des tours verticalement

        // On sépare la rotation X (verticale) et Y (Horizontal) pour éviter que le perso se penche en avant ou arrière

        // appliquer rotation player (Y seulement)
        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

        // appliquer rotation caméra (X seulement)
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        if (Input.GetButtonDown("Reload"))
        {
            weapon.Reload();
        }

        else

        if (Input.GetButton("Fire1"))
        {
            weapon.Fire();
        }
    }
}
