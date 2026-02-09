using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BaseEnemy : NetworkBehaviour, ICanDie, IDamageable
{
    // [SerializeField] private EnemyBehavior behaviour;
    public EnemyStats stats;

    public bool isDead { get; set; } = false;

    public float maxHealth { get; set; }

    private GameObject m_Target;
    public NavMeshAgent m_Agent;
    private Animator m_Animator;

    public bool isAttacking = false;
    private bool hasFoundCapsule = false;
    public LayerMask targetMask;

    public NetworkVariable<float> Health = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    public float health
    {
        get => Health.Value;
        set
        {
            if (IsServer) Health.Value = value;
        }
    }

    public override void OnNetworkSpawn()
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

        maxHealth = stats.health;
        m_Agent.stoppingDistance = stats.attackRange;
        m_Agent.speed = stats.moveSpeed;

        SecretFeature.OnCheatCode += startDancingRpc;

        if (IsServer)
        {
            Health.Value = stats.health;
        }
       
        NetworkObject.DestroyWithScene = true;
    }

    void Update()
    {
        if (!IsServer) return;

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

            if (Health.Value <= 0)
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
        TakeDamageRpc(damage);
    }

    [Rpc(SendTo.Server)]
    void TakeDamageRpc(float damage)
    {
        Health.Value -= damage;
    }

    public void Die()
    {
        DieRpc();
    }

    [Rpc(SendTo.Everyone)]
    private void DieRpc()
    {
        m_Agent.isStopped = true;
        m_Animator.SetTrigger("Die");
    }

    public void DestroyOnNetwork()
    {
        if (!IsServer)
        {
            // If client, call serveur
            DestroyServerRpc();
        } else
        {
            // If server, destroy directly
            DestroyForEveryone();
        }
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    private void DestroyServerRpc()
    {
        DestroyForEveryone();
    }

    private void DestroyForEveryone()
    {
        NetworkObject netObj = gameObject.GetComponent<NetworkObject>();
        netObj.Despawn(destroy: true);
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
                PlayAttackAnimationRpc();
            }
        }
    }

    [Rpc(SendTo.Everyone)]
    void PlayAttackAnimationRpc()
    {
        m_Animator.SetTrigger("Attack");
    }

    public void LookAtTarget()
    {
        if (m_Target != null)
        {
            Vector3 direction = m_Target.transform.position - gameObject.transform.position;
            gameObject.transform.rotation = Quaternion.LookRotation(direction);

            var yRotation = gameObject.transform.eulerAngles.y;
            gameObject.transform.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }

    public int GetCost()
    {
        return stats.cost;
    }

    public Boolean isServer()
    {
        return IsServer;
    }



    //------------------------- ULTRA SECRET --------------------------

    bool IsDancing()
    {
        return m_Animator.GetCurrentAnimatorStateInfo(0).IsName("secretState");
    }


    [Rpc(SendTo.Everyone)]
    void startDancingRpc(Quaternion rotation)
    {
        Debug.Log(rotation);
        isDead = true;

        m_Agent.isStopped = true;
        transform.rotation = Quaternion.identity;
        m_Animator.applyRootMotion = true;
        m_Animator.SetTrigger("Dance");
    }
}
