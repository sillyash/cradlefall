using UnityEngine;

public class BaseModule : MonoBehaviour
{
    [SerializeField] private ModuleBehavior behaviour;
    public ModuleStats stats;

    private bool broken = false;
    private float health;

    public void Start()
    {
        if (!stats)
        {
            enabled = false;
            Debug.Log($"module = {this.name} n'a pas de stats");
        }
        if (!behaviour)
        {
            enabled = false;
            Debug.Log($"module = {this.name} n'a pas de behaviour");
        }

        health = stats.health;
    }

    public void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
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
        behaviour.OnRepair(this);
    }
}
