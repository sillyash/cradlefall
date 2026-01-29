using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using Constants;

public class NetworkBootstrap : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // Track client connections
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        
        NetworkManager.Singleton.OnServerStarted += OnServerStarted;
    }

    private void OnServerStarted()
    {
        Debug.Log("[NetworkBootstrap] Server started! Waiting for clients...");
    }

    private void OnClientConnected(ulong clientId)
    {
        Debug.Log($"[NetworkBootstrap] Client {clientId} connected!");
        
        if (NetworkManager.Singleton.IsServer)
        {
            // Only spawn players in the actual game scene, not in lobby
            string currentScene = SceneManager.GetActiveScene().name;
            if (currentScene == Scenes.MainGameMulti)
            {
                PlayerManager.Instance?.SpawnPlayerForClient(clientId);
            }
        }
    }

    private void OnClientDisconnected(ulong clientId)
    {
        Debug.LogWarning($"[NetworkBootstrap] Client {clientId} disconnected!");
        
        if (NetworkManager.Singleton.IsServer)
        {
            // Server despawns this player
            PlayerManager.Instance?.DespawnPlayerForClient(clientId);
        }
    }
}
