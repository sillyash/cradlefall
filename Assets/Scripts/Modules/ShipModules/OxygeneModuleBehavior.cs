using UnityEngine;

[CreateAssetMenu(menuName = "Modules/Behaviours/Timer")]
public class TimerModuleBehavior : ModuleBehavior
{
    public override void OnBreak(BaseModule module)
    {
        // TODO
        Debug.Log("Module �lectricit� cass� !");
    }

    public override void OnRepair(BaseModule module)
    {
        // TODO
        throw new System.NotImplementedException();
    }
}
