using Unity.VisualScripting;
using UnityEngine;

public class EnemySphereBehavior : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Qu'à la fin de l'envoi, je touche!");
    }
}
