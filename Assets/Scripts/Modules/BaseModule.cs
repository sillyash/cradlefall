using UnityEngine;

public class BaseModule : MonoBehaviour
{
    [SerializeField] private ModuleBehavior behaviour;
    public ModuleStats stats;

    public bool broken = false;
    [SerializeField] private float health;

    public void Start()
    {
        // TODO Debug TO REMOVE
        // Implémenter ici (ou dans une meilleure func) la logique de casse des modules (petites chances chaque sec par exemple)
        //Invoke(nameof(Break), 3f);
    }

    public void Break()
    {
        Debug.Log("The module has BROKEN !");
        if (broken) return;
        broken = true;
        behaviour.OnBreak(this);
    }

    public void Repair()
    {
        Debug.Log("Module REPAIRED");
        if (!broken) return;
        broken = false;
        health = stats.health;
        behaviour.OnRepair(this);
    }
}
