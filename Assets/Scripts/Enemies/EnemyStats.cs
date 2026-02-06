using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Enemy/Stats")]
public class EnemyStats : ScriptableObject
{
    public float health;
    public float attackRange;
    public float moveSpeed;
    public float damage;
    public float attackCooldown;
    public float detectionRange;

    // For wave spawner, we need a cost value
    public int cost;
}
