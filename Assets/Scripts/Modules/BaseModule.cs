using UnityEngine;

public class BaseModule : MonoBehaviour
{
    [SerializeField] private ModuleBehavior behaviour;
    public ModuleStats stats;

    private bool broken = false;

    public void Start()
    {
        // TODO Debug TO REMOVE
        // Implémenter ici (ou dans une meilleure func) la logique de casse des modules (petites chances chaque sec par exemple)
        //Invoke(nameof(Break), 3f);
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
