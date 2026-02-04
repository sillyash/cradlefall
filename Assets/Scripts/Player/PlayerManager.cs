using Constants;
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

    [SerializeField] private GameObject playerPrefab; // Reference to Player.prefab
    [SerializeField] private GameObject deathCamPrefab;
    [SerializeField] private Vector3 spawnPosition = Vector3.zero;
    public Dictionary<ulong, basePlayer> activePlayers = new();
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

        // Instantiate player locally on server
        GameObject playerObj = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
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
        
        Debug.Log($"[PlayerManager] Spawned player for client {clientId}");
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
        Debug.Log("Dispawn player");

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
        Debug.Log($"[PlayerManager] Despawned player for client {clientId}");
    }

    /// <summary>
    /// Respawn a player after a delay (used for multiple lives, etc.)
    /// </summary>
    /// <param name="clientId">
    ///  The client ID of the player to respawn
    /// </param>
    private IEnumerator RespawnPlayersAfterDelay(ulong clientId)
    {
        Debug.Log("Respawn");
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
        Debug.Log($"[PlayerManager] ICIIII");

        if (!IsServer) return;

        Debug.Log($"[PlayerManager] Player {clientId} died!");

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
        Debug.Log($"[NetworkBootstrap] Client {clientId} connected!");
        NbOfActiveClients.Value++;
        if (NetworkManager.Singleton.IsServer && GameManager.instance?.currentState.Value == GameState.Playing)
        {
            // Server spawns player for this client
            SpawnPlayerForClient(clientId);
        }
    }

    private void OnClientDisconnected(ulong clientId)
    {
        Debug.LogWarning($"[NetworkBootstrap] Client {clientId} disconnected!");

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
        Debug.Log("[PlayerManager] Spawning players for all connected clients...");

        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            Debug.Log("[PlayerManager] Spawning " + clientId);
            if (!PlayerManager.Instance.activePlayers.ContainsKey(clientId))
            {
                Debug.Log("[PlayerManager] SpawningAAA " + clientId);
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
