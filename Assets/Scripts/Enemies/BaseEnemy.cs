using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BaseEnemy : MonoBehaviour, ICanDie, IDamageable
{
    // [SerializeField] private EnemyBehavior behaviour;
    public EnemyStats stats;

    public bool isDead { get; set; } = false;
    public float health { get; set; }
    public float maxHealth { get; set; }

    private GameObject m_Target;
    private NavMeshAgent m_Agent;
    private Animator m_Animator;

    public bool isAttacking = false;
    private bool hasFoundCapsule = false;
    public LayerMask targetMask;
    
    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
        m_Animator = GetComponent<Animator>();
        
        if (!stats)
        {
            Debug.LogError("Le script BaseEnemy du prefab Enemy n�a pas de ScriptableObject de statistiques assign�.");
            enabled = false;
            return;
        }
/*        if (!behaviour)
        {
            Debug.LogError("Le script BaseEnemy du prefab Enemy n�a pas de ScriptableObject de behaviour assign�.");
            enabled = false;
            return;
        }*/
        

        health = stats.health;
        maxHealth = stats.health;
        m_Agent.stoppingDistance = stats.attackRange;
        m_Agent.speed = stats.moveSpeed;

    }

    void Update()
    {
        if (!isDead)
        {
            if (m_Agent.velocity.magnitude >= 0.1f)
            {
                m_Animator.SetBool("isRunning", true);
            }
            else
            {
                m_Animator.SetBool("isRunning", false);
            }

            if (health <= 0)
            {
                isDead = true;
                Die();
            }
            else if (!isAttacking)
            {
                if (!hasFoundCapsule)
                {
                    m_Target = UpdateTarget();
                }
                TrackTarget();
            }
        }
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("hit received");
        health -= damage;
    }

    public void Die()
    {
        m_Agent.isStopped = true;
        m_Animator.SetTrigger("Die");
    }

    public GameObject AcquireClosestCapsule()
    {
        // recup caisson
        // test distance caisson player
        // set target caisson/player

        GameObject[] capsules = GameObject.FindGameObjectsWithTag("Capsule");

        float distance;
        float shorterDistance;
        GameObject target;
        if (capsules != null) 
        {
            target = capsules[0];
            shorterDistance = Vector3.Distance(transform.position, target.transform.position);
            for (int i = 0; i < capsules.Length; i++)
            {
                distance = Vector3.Distance(transform.position, capsules[i].transform.position);
                if (distance < shorterDistance)
                {
                    target = capsules[i];
                    shorterDistance = distance;
                }
            }
            return target;
        }
        return null;
    }

    public GameObject UpdateTarget()
    {
        List<Collider> hits = Physics.OverlapSphere(transform.position, stats.detectionRange, targetMask).ToList();
        
        float closestDist = Mathf.Infinity;
        GameObject closest = null;
        List<Collider> playerHits = new List<Collider>();
        if (hits != null)
        {
            foreach (var h in hits)
            {
                if (h.gameObject.layer == LayerMask.NameToLayer("Capsule"))
                {
                    float dist = Vector3.Distance(transform.position, h.transform.position);
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        closest = h.gameObject;
                    }
                }
                else playerHits.Add(h);
            }
            if (closest != null) hasFoundCapsule = true;
            else if (playerHits.Count > 0)
            {
                foreach (var h in playerHits)
                {
                    if (h.gameObject.layer == LayerMask.NameToLayer("Player"))
                    {
                        float dist = Vector3.Distance(transform.position, h.transform.position);
                        if (dist < closestDist)
                        {
                            closestDist = dist;
                            closest = h.gameObject;
                        }
                    }
                }
            }
        } 
        if (closest == null) closest = AcquireClosestCapsule();
        return closest;
    }

    public void TrackTarget()
    {
        if (m_Target != null)
        {
            Vector3 closestPointOnTarget = m_Target.GetComponent<Collider>().ClosestPoint(transform.position);
            float distance = Vector3.Distance(transform.position, closestPointOnTarget);

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
    public void LookAtTarget()
    {
        Vector3 direction = m_Target.transform.position - gameObject.transform.position;
        gameObject.transform.rotation = Quaternion.LookRotation(direction);

        var yRotation = gameObject.transform.eulerAngles.y;
        gameObject.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public int GetCost()
    {
        return stats.cost;
    }
}
