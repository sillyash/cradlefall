using UnityEngine;

[CreateAssetMenu(fileName = "EnemiesStats", menuName = "Enemies/Stats")]
public class EnemiesStats : ScriptableObject
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
