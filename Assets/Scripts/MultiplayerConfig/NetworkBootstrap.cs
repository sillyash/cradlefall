using Unity.Netcode;
using UnityEngine;

public class NetworkBootstrap : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        NetworkManager.Singleton.OnClientConnectedCallback += (clientId) =>
        {
            Debug.Log($"Client {clientId} connecté !");
        };

        NetworkManager.Singleton.OnServerStarted += () =>
        {
            Debug.Log("Server started !");
        };
    }
}
