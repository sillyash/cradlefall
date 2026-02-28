using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LightManager : NetworkBehaviour
{
    public static LightManager instance { get; private set; }
    [SerializeField] private float delayBetweenLights = 0.15f;
    private GameObject[] AllLights;


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

    public void TurnOffEveryLights()
    {
        if (!IsServer) return;
        TurnOffEveryLightsClientRpc();
    }

    public void TurnOnEveryLights()
    {
        if (!IsServer) return;
        TurnOnEveryLightsClientRpc();
    }

    [ClientRpc]
    private void TurnOffEveryLightsClientRpc()
    {
        AllLights = GameObject.FindGameObjectsWithTag("Module-Light");

        foreach (GameObject light in AllLights)
        {
            light.SetActive(false);
        }
    }

    [ClientRpc]
    private void TurnOnEveryLightsClientRpc()
    {
        // Ici on peut pas faire des recherche par Tag parceque si l'object est pas Active, il est pas retrouvable par Tag
        StartCoroutine(TurnOnLightsCoroutine());
    }

    private IEnumerator TurnOnLightsCoroutine()
    {
        List<GameObject> lightsList = new List<GameObject>(AllLights);

        while (lightsList.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, lightsList.Count);
            GameObject randomLight = lightsList[index];
            randomLight.SetActive(true);

            lightsList.RemoveAt(index);

            yield return new WaitForSeconds(delayBetweenLights);
        }
    }
}
