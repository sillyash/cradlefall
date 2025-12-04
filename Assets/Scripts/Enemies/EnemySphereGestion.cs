using UnityEngine;

public class EnemySphereGestion : MonoBehaviour
{
    baseEnemies enemy;
    SphereCollider main;

    void Start()
    {
        enemy = gameObject.GetComponent<baseEnemies>();
        main = gameObject.GetComponentInChildren<SphereCollider>(true);
    }

    public void ActivateHand()
    {
        main.gameObject.SetActive(true);
    }

    public void DeactivateHand()
    {
        main.gameObject.SetActive(false);
    }
}
