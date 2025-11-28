using UnityEngine;

public class BaseModule : MonoBehaviour
{
    [SerializeField] private ModuleBehavior behaviour;
    public ModuleStats stats;

    public bool broken = false;
    [SerializeField] private float health;

    public void Start()
    {
        if (!stats)
        {
            enabled = false;
            Debug.LogError($"module = {this.name} n'a pas de stats");
            return;
        }
        if (!behaviour)
        {
            enabled = false;
            Debug.LogError($"module = {this.name} n'a pas de behaviour");
            return;
        }

        health = stats.health;
    }

    public void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        if (broken) return;
        health -= damage;
        if ( health <= 0 )
        {
            Break();
        }
    }

    public void Break()
    {
        if (broken) return;
        broken = true;
        behaviour.OnBreak(this);
    }

    public void Repair()
    {
        if (!broken) return;
        broken = false;
        health = stats.health;
        behaviour.OnRepair(this);
    }
}
