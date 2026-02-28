using System;
using Unity.Netcode;
using UnityEngine;

public class ChargingStationManager : NetworkBehaviour
{
    public static ChargingStationManager instance { get; private set; }

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

    public void TurnOffAllStations()
    {
        if (!IsServer) return;
        TurnOffAllStationsClientRpc();
    }

    [ClientRpc]
    private void TurnOffAllStationsClientRpc()
    {
        GameObject[] stations = GameObject.FindGameObjectsWithTag("Charging-Station");

        foreach (GameObject station in stations)
        {
            ChargingStation chargingStation = station.GetComponent<ChargingStation>();
            chargingStation.Broken.Value = true;
        }
    }

    public void TurnOnAllStations()
    {
        if (!IsServer) return;
        TurnOnAllStationsClientRpc();
    }

    [ClientRpc]
    private void TurnOnAllStationsClientRpc()
    {
        GameObject[] stations = GameObject.FindGameObjectsWithTag("Charging-Station");

        foreach (GameObject station in stations)
        {
            ChargingStation chargingStation = station.GetComponent<ChargingStation>();
            chargingStation.Broken.Value = false;
        }
    }
}
