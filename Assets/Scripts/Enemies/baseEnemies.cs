using UnityEngine;

public class baseEnemies : MonoBehaviour
{
    // [SerializeField] private EnemiesBehavior behaviour;
    [SerializeField] private EnemiesStats stats;

    private float health;

    void Start()
    {
        health = stats.health;
    }

    void Update()
    {
        if ( health <= 0 )
        {
            Die();
        }
    }

    public void TakeDammage(float dammage)
    {
        health -= dammage;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
