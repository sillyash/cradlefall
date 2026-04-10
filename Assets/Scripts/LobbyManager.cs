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

    public NetworkList<ulong> readyClients = new NetworkList<ulong>();

    public void Awake()
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
        if (!IsServer) return;
        readyClients.Clear();
    }

	public bool IsReady(ulong clientId)
	{
		return readyClients.Contains(clientId);
	}

	public int ReadyCount => readyClients.Count;
	public bool AllReady => PlayerManager.Instance?.NbOfActiveClients.Value >= minPlayersToStart
						&& readyClients.Count == PlayerManager.Instance?.NbOfActiveClients.Value;

	public void SetLocalReady(int roleIndex)
	{
        if (!IsClient) return;
        SetReadyServerRpc(roleIndex);
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    private void SetReadyServerRpc(int roleIndex, RpcParams rpcParams = default)
    {
        ulong clientId = rpcParams.Receive.SenderClientId;
        PlayerManager.Instance.SetRoleForClient(roleIndex, clientId);

        if (!IsReady(clientId))
        {
            readyClients.Add(clientId);
        }
        else
        {
            readyClients.Remove(clientId);
        }

        if (AllReady)
            GameManager.instance?.PlayGame();
    }
}
