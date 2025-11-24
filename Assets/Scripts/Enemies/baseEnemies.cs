using UnityEngine;

public class baseEnemies : MonoBehaviour
{
    [SerializeField] private EnemiesBehavior behaviour;
    [SerializeField] private EnemiesStats stats;

    void Start()
    {
        
    }

    void Update()
    {
        if ( stats.health <= 0 )
        {
            Die();
        }
    }

    public void TakeDammage(float dammage)
    {
        stats.health -= dammage;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
