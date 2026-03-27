using UnityEngine;
using UnityEngine.UI;

public class CrossScript : MonoBehaviour
{
    [SerializeField] private Image crosshairDefault;
    [SerializeField] private Image crosshairTarget;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private string enemyTag = "MainEnemy"; 
    [SerializeField] private float rayDistance = 100f;

    private void Update()
    {
        if (playerCamera == null) return;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            if (hit.collider.CompareTag(enemyTag))
            {
                crosshairDefault.enabled = false;
                crosshairTarget.enabled = true;
                return;
            }
        }

        // Si rien ou pas un ennemi
        crosshairDefault.enabled = true;
        crosshairTarget.enabled = false;
    }
}
