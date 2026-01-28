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
    [SerializeField] private Vector3 spawnPosition = Vector3.zero;
    
    private Dictionary<ulong, basePlayer> activePlayers = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            enabled = false;
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
        networkObj.SpawnWithOwnership(clientId);

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
    /// Called when a player dies (server-side)
    /// </summary>
    /// <param name="clientId">
    /// The client ID of the dead player
    /// </param>
    public void OnPlayerDeath(ulong clientId)
    {
        if (!IsServer) return;

        Debug.Log($"[PlayerManager] Player {clientId} died!");
        
        // Immediately despawn the player
        DespawnPlayerForClient(clientId);
        
        // TODO: In future, show lobby UI and wait for respawn signal
        // For now, we could auto-respawn after delay:
        // StartCoroutine(RespawnPlayerAfterDelay(clientId, 3f));
    }

    /// <summary>
    /// Respawn a player (used for lobbies, multiple lives, etc.)
    /// </summary>
    /// <param name="clientId">
    ///  The client ID of the player to respawn
    /// </param>
    public void RespawnPlayerForClient(ulong clientId)
    {
        if (!IsServer) return;

        // Make sure old player is despawned first
        if (activePlayers.ContainsKey(clientId))
        {
            DespawnPlayerForClient(clientId);
        }

        // Spawn new player instance
        SpawnPlayerForClient(clientId);
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
}
