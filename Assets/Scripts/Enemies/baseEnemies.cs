using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class baseEnemies : MonoBehaviour
{
    // [SerializeField] private EnemiesBehavior behaviour;
    [SerializeField] private EnemiesStats stats;

    public Transform Target;
    public float AttackDistance;

    private UnityEngine.AI.NavMeshAgent m_Agent;
    private float m_Distance;

    private float health;

    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();

        if (!stats)
        {
            Debug.LogError("Le script baseEnemies du prefab Enemy n�a pas de ScriptableObject de statistiques assign�.");
            enabled = false;
            return;
        }
/*        if (!behaviour)
        {
            Debug.LogError("Le script baseEnemies du prefab Enemy n�a pas de ScriptableObject de behaviour assign�.");
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

    public void TakeDammage(float dammage)
    {
        health -= dammage;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
