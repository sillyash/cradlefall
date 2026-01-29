using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Constants;

/// <summary>
/// Manages the lobby state and player readiness in a multiplayer game.
/// </summary>
public class LobbyManager : NetworkBehaviour
{
	public static LobbyManager Instance { get; private set; }

	[SerializeField] private int minPlayersToStart = 1;

	private readonly HashSet<ulong> connectedClients = new();
	private NetworkList<ulong> readyClients;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}

		Instance = this;
		DontDestroyOnLoad(gameObject);

		readyClients = new NetworkList<ulong>();
    }

	public override void OnNetworkSpawn()
	{
		if (!IsServer)
		{
			return;
		}

		connectedClients.Clear();
		foreach (var clientId in NetworkManager.Singleton.ConnectedClientsIds)
		{
			connectedClients.Add(clientId);
		}

		NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
		NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
	}

	public override void OnNetworkDespawn()
	{
		if (NetworkManager.Singleton != null)
		{
			NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
			NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnected;
		}

		base.OnNetworkDespawn();
	}

	private void OnClientConnected(ulong clientId)
	{
		if (!IsServer) return;

		connectedClients.Add(clientId);
	}

	private void OnClientDisconnected(ulong clientId)
	{
		if (!IsServer) return;

		connectedClients.Remove(clientId);
		readyClients.Remove(clientId);
	}

	public bool IsReady(ulong clientId)
	{
		return readyClients.Contains(clientId);
	}

	public int ReadyCount => readyClients.Count;
	public int ConnectedCount => connectedClients.Count;

	public bool AllReady => connectedClients.Count >= minPlayersToStart
						&& readyClients.Count == connectedClients.Count;

	public void SetLocalReady(bool ready)
	{
		if (!IsClient) return;
		SetReadyServerRpc(ready);
	}

	public void TryStartGame()
	{
		if (!IsServer) return;
		if (!AllReady) return;

		StartGameServer();
	}

	private void StartGameServer()
	{
		Debug.Log("[LobbyManager] All players ready! Starting game...");
		
		// Spawn all players before transitioning to game scene
		PlayerManager.Instance?.SpawnAllPlayers();
		
		// Change to game scene
		GameManager.instance?.ChangeScene(Scenes.MainGameMulti);
	}

	[Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
	private void SetReadyServerRpc(bool ready, RpcParams rpcParams = default)
	{
		if (!IsServer) return;

		ulong clientId = rpcParams.Receive.SenderClientId;

		if (ready)
		{
			if (!readyClients.Contains(clientId))
			{
				readyClients.Add(clientId);
			}
		}
		else
		{
			readyClients.Remove(clientId);
		}

		if (AllReady)
		{
			StartGameServer();
		}
	}
}
