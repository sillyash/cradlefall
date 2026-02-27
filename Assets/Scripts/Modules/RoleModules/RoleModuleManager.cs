using System.Collections.Generic;
using Unity.Netcode;

public class RoleModuleManager : NetworkBehaviour
{
    public static RoleModuleManager instance = null;
    public BaseModule DPSModule;
    public BaseModule TechnicienModule;
    public BaseModule SoigneurModule;
    public BaseModule TankModule;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SubToModule(BaseModule module)
    {
        RoleModuleBehaviour behaviour = (RoleModuleBehaviour) module.behaviour;

        behaviour.OnBreakEvent += OnModuleEvent;
        behaviour.OnRepairEvent += OnModuleEvent;

        assignModule(module);
    }

    private void OnModuleEvent(BaseModule module)
    {
        // Do something ?
    }

    private void assignModule(BaseModule module)
    {
        RoleModuleBehaviour behaviour = (RoleModuleBehaviour) module.behaviour;
        switch (behaviour.role)
        {
            case RoleModuleBehaviour.Role.DPS:
                DPSModule = module;
                break;
            case RoleModuleBehaviour.Role.Technicien:
                TechnicienModule = module;
                break;
            case RoleModuleBehaviour.Role.Soigneur:
                SoigneurModule = module;
                break;
            case RoleModuleBehaviour.Role.Tank:
                TankModule = module;
                break;
        }
    }
}
