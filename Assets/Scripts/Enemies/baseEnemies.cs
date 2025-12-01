using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class baseEnemies : MonoBehaviour, ICanDie
{
    // [SerializeField] private EnemiesBehavior behaviour;
    [SerializeField] private EnemiesStats stats;

    public bool isDead { get; set; } = false;
    public float health;

    private GameObject m_Target;
    private NavMeshAgent m_Agent;
    private Animator m_Animator;

    public bool isAttacking = false;

    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Animator = GetComponent<Animator>();

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
        if (!m_Target)
        {
            Debug.LogError("Le script baseEnemies du prefab Enemy n'a pas de Target assigné.");
            enabled = false;
            return;
        }

        health = stats.health;
        
    }

    void Update()
    {
        if (m_Agent.velocity.magnitude >= 0.1f)
        {
            m_Animator.SetBool("isRunning", true);
        }
        else
        {
            m_Animator.SetBool("isRunning", false);
        }

        if ( health <= 0 )
        {
            Die();
        } else if (!isAttacking ) 
        {
            TrackTarget();
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    public void Die()
    {
        m_Animator.SetTrigger("Die");
    }

    public void TrackTarget()
    {
        if (m_Target != null)
        {
            float distance = Vector3.Distance(transform.position, m_Target.transform.position);

            if (distance > stats.attackRange)
            {
                m_Agent.SetDestination(m_Target.transform.position);
                m_Agent.isStopped = false;
            }
            else
            {
                m_Agent.isStopped = true;
                isAttacking = true;
                m_Animator.SetTrigger("Attack");
            }
        }
    }
}
