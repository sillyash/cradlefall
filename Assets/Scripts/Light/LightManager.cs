using Unity.Netcode;
using UnityEngine;

public class LightManager : NetworkBehaviour
{
    public static LightManager instance { get; private set; }
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

    public void MakeCapsulesBlink()
    {
        if (!IsServer) return;
        MakeCapsulesBlinkClientRpc();
    }

    public void StopCapsulesBlink()
    {
        if (!IsServer) return;
        StopCapsulesBlinkClientRpc();
    }

    [ClientRpc]
    private void MakeCapsulesBlinkClientRpc()
    {
        GameObject[] capsules = GameObject.FindGameObjectsWithTag("Capsule");

        foreach (GameObject capsule in capsules)
        {
            BlinkingLightController blinkingController = capsule.GetComponentInChildren<BlinkingLightController>();
            blinkingController.SwitchOnBlinking();
        }
    }

    [ClientRpc]
    private void StopCapsulesBlinkClientRpc()
    {
        GameObject[] capsules = GameObject.FindGameObjectsWithTag("Capsule");

        foreach (GameObject capsule in capsules)
        {
            BlinkingLightController blinkingController = capsule.GetComponentInChildren<BlinkingLightController>();
            blinkingController.SwitchOffBlinking();
        }
    }
}
