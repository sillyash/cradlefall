using UnityEngine;
using UnityEngine.UI;

public class CrossScript : MonoBehaviour
{
    [SerializeField] private Image crosshairTarget;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private string enemyTag = "MainEnemy"; 
    [SerializeField] private float rayDistance = 100f;

    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color enemyColor = Color.red;

    private void Update()
    {
        if (playerCamera == null) return;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            if (hit.collider.CompareTag(enemyTag))
            {
      
                crosshairTarget.color = enemyColor;
                return;
            }
        }

     
       crosshairTarget.color = defaultColor;
    }
}
