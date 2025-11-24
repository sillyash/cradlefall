using UnityEngine;

[CreateAssetMenu(menuName = "Modules/Behaviours/Electricity")]
public class ElectricityModuleBehavior : ModuleBehavior
{
    public override void OnBreak(BaseModule module)
    {
        // TODO
        Debug.Log("Module Électricité cassé !");
    }

    public override void OnRepair(BaseModule module)
    {
        // TODO
        throw new System.NotImplementedException();
    }
}
