using UnityEngine;

public class EnemySphereGestion : MonoBehaviour
{
    BaseEnemy enemy;
    SphereCollider main;

    void Start()
    {
        enemy = gameObject.GetComponent<BaseEnemy>();
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
