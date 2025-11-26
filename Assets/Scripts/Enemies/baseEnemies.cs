using UnityEngine;

public class baseEnemies : MonoBehaviour
{
    // [SerializeField] private EnemiesBehavior behaviour;
    [SerializeField] private EnemiesStats stats;

    private float health;

    void Start()
    {
        if (!stats)
        {
            Debug.LogError("Le script baseEnemies du prefab Enemy n’a pas de ScriptableObject de statistiques assigné.");
            enabled = false;
            return;
        }
/*        if (!behaviour)
        {
            Debug.LogError("Le script baseEnemies du prefab Enemy n’a pas de ScriptableObject de behaviour assigné.");
            enabled = false;
            return;
        }*/

        health = stats.health;
    }

    void Update()
    {
        if ( health <= 0 )
        {
            Die();
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
