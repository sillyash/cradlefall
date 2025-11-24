using UnityEngine;

public class baseEnemies : MonoBehaviour
{
    // [SerializeField] private EnemiesBehavior behaviour;
    [SerializeField] private EnemiesStats stats;

    [SerializeField] private float health;
    [SerializeField] private float attackRange;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float damage;
    [SerializeField] private float attackCooldown;

    void Start()
    {
        health = stats.health;
        attackRange = stats.attackRange;
        moveSpeed = stats.moveSpeed;
        damage = stats.damage;
        attackCooldown = stats.attackCooldown;
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
