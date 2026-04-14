using Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// Manages player spawning, despawning, and tracking in a multiplayer game.
/// </summary>
public class PlayerManager : NetworkBehaviour
{
    public static PlayerManager Instance { get; private set; }

    [SerializeField] private List<GameObject> playerPrefabs;
    [SerializeField] private GameObject deathCamPrefab;
    [SerializeField] private Vector3 spawnPosition = Vector3.zero;
    public Dictionary<ulong, basePlayer> activePlayers = new();
    public Dictionary<ulong, GameObject> activePlayersPrefabs = new();
    public Dictionary<ulong, NetworkObject> activeDeathCameras = new();

    public NetworkVariable<int> NbOfActiveClients = new(
        0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            // Track client connections
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        }
    }

    public void SetRoleForClient(int roleIdex, ulong ownerClientId)
    {
        switch (Roles.RolesMap[roleIdex])
        {
            case Roles.DPS:
                activePlayersPrefabs[ownerClientId] = playerPrefabs[0];
                break;
            case Roles.TECH:
                activePlayersPrefabs[ownerClientId] = playerPrefabs[1];
                break;
            case Roles.HEAL:
                activePlayersPrefabs[ownerClientId] = playerPrefabs[2];
                break;
        }
    }

    /// <summary>
    /// Spawn a new player for the given client
    /// </summary>
    /// <param name="clientId">
    /// The client ID to spawn the player for
    /// </param>
    public void SpawnPlayerForClient(ulong clientId)
    {
        if (!IsServer) return;
        if (activePlayers.ContainsKey(clientId))
        {
            Debug.LogWarning($"[PlayerManager] Player for client {clientId} already exists!");
            return;
        }

        GameObject role;
        if(activePlayersPrefabs.TryGetValue(clientId, out var foundRole))
        {
            role = foundRole;
        }
        else
        {
            role = playerPrefabs[0]; //Defaluts to dps
            activePlayersPrefabs[clientId] = role;
        }

        // Instantiate player locally on server
        GameObject playerObj = Instantiate(role, spawnPosition, Quaternion.identity);
        NetworkObject networkObj = playerObj.GetComponent<NetworkObject>();
        basePlayer basePlayerComp = playerObj.GetComponent<basePlayer>();

        if (networkObj == null || basePlayerComp == null)
        {
            Debug.LogError("[PlayerManager] Player prefab missing NetworkObject or basePlayer component!");
            Destroy(playerObj);
            return;
        }
        // Spawn with ownership (client becomes owner)
        networkObj.SpawnAsPlayerObject(clientId, true);

        // Track this player
        activePlayers[clientId] = basePlayerComp;
    }

    // Despawn a player when they die or disconnect
    public void DespawnPlayerForClient(ulong clientId)
    {
        if (!IsServer) return;

        if (!activePlayers.TryGetValue(clientId, out basePlayer player))
        {
            Debug.LogWarning($"[PlayerManager] No active player found for client {clientId}");
            return;
        }

        // Despawn the NetworkObject
        NetworkObject networkObj = player.GetComponent<NetworkObject>();
        if (networkObj != null)
        {
            networkObj.Despawn(destroy: true);
        }
        else
        {
            Destroy(player.gameObject);
        }

        activePlayers.Remove(clientId);
    }

    /// <summary>
    /// Respawn a player after a delay (used for multiple lives, etc.)
    /// </summary>
    /// <param name="clientId">
    ///  The client ID of the player to respawn
    /// </param>
    private IEnumerator RespawnPlayersAfterDelay(ulong clientId)
    {
        yield return new WaitForSeconds(10f);

        // Make sure old player is despawned first
        if (activePlayers.ContainsKey(clientId))
        {
            DespawnPlayerForClient(clientId);
        }
        if (activeDeathCameras.TryGetValue(clientId, out NetworkObject deatCam))
        {
            deatCam.Despawn(true);
        }
        PlayerManager.Instance?.SpawnPlayerForClient(clientId);
    }

    /// <summary>
    /// Called when a player dies (server-side)
    /// </summary>
    /// <param name="clientId">
    /// The client ID of the dead player
    /// </param>
    public void OnPlayerDeath(ulong clientId)
    {
        if (!IsServer) return;

        // Immediately despawn the player
        DespawnPlayerForClient(clientId);

        GameObject deathCam = Instantiate(deathCamPrefab, new Vector3(-26f, 8f, -46f), new Quaternion(0.14f, 0.15f, -0.02f, 0.97f));
        NetworkObject deathCamNetworkObj = deathCam.GetComponent<NetworkObject>();
        deathCamNetworkObj.SpawnWithOwnership(clientId, true);
        activeDeathCameras[clientId] = deathCamNetworkObj;

        StartCoroutine(RespawnPlayersAfterDelay(clientId));
    }

    private void OnClientConnected(ulong clientId)
    {
        NbOfActiveClients.Value++;
        if (NetworkManager.Singleton.IsServer && GameManager.instance?.currentState.Value == GameState.Playing)
        {
            // Server spawns player for this client
            SpawnPlayerForClient(clientId);
        }
    }

    private void OnClientDisconnected(ulong clientId)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            // Server despawns this player
            DespawnPlayerForClient(clientId);
        }
    }

    /// <summary>
    /// Spawn players for all connected clients (used when transitioning from lobby to game)
    /// </summary>
    public void SpawnAllPlayers()
    {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (!PlayerManager.Instance.activePlayers.ContainsKey(clientId))
            {
                SpawnPlayerForClient(clientId);
            }
        }
    }

    /// <summary>
    /// Get active player by client ID
    /// </summary>
    /// <param name="clientId">
    /// The client ID of the player to retrieve
    /// </param>
    /// <returns>
    /// The basePlayer instance for the given client ID, or null if not found
    /// </returns>
    public basePlayer GetPlayerByClientId(ulong clientId)
    {
        activePlayers.TryGetValue(clientId, out basePlayer player);
        return player;
    }

    /// <summary>
    /// Get all active players
    /// </summary>
    /// <returns>
    /// An enumerable of all active basePlayer instances
    /// </returns>
    public IEnumerable<basePlayer> GetAllPlayers()
    {
        return activePlayers.Values;
    }

    /// <summary>
    /// Get all active clients ids
    /// </summary>
    /// <returns>
    /// An enumerable of all active clients ids
    /// </returns>
    public IEnumerable<ulong> GetAllClients()
    {
        return activePlayers.Keys;
    }
}
