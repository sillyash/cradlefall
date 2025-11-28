using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class baseEnemies : MonoBehaviour
{
    // [SerializeField] private EnemiesBehavior behaviour;
    [SerializeField] private EnemiesStats stats;

    public Transform Target;

    private NavMeshAgent m_Agent;

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
        if (!Target)
        {
            Debug.LogError("Le script baseEnemies du prefab Enemy n'a pas de Target assigné.");
            enabled = false;
            return;
        }

        health = stats.health;
    }

    void Update()
    {
        if ( health <= 0 )
        {
            Die();
        } else
        {
            TrackPlayer();
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

    public void TrackPlayer()
    {
        if (Target != null)
        {
            float distance = Vector3.Distance(transform.position, Target.position);

            if (distance > stats.attackRange)
            {
                m_Agent.SetDestination(Target.position);
                m_Agent.isStopped = false;
            }
            else
            {
                m_Agent.isStopped = true;
                // behaviour.Attack();
            }
        }
    }
}
