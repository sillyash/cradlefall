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

	private NetworkList<ulong> readyClients = new NetworkList<ulong>();

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Destroy(gameObject);
        }
    }

    public override void OnNetworkSpawn()
	{
        if (!IsServer) return;
	}

	public bool IsReady(ulong clientId)
	{
		return readyClients.Contains(clientId);
	}

	public int ReadyCount => readyClients.Count;
	public bool AllReady => PlayerManager.Instance?.NbOfActiveClients.Value >= minPlayersToStart
						&& readyClients.Count == PlayerManager.Instance?.NbOfActiveClients.Value;

	public void SetLocalReady(bool ready, int roleIdex)
	{
        if (IsClient)
        {
            PlayerManager.Instance.SetRoleForClient(roleIdex, OwnerClientId);
		    SetReadyServerRpc(ready);
        } else
        {
            if (ready)
            {
                if (!readyClients.Contains(OwnerClientId))
                {
                    readyClients.Add(OwnerClientId);
                }
            }
            else
            {
                readyClients.Remove(OwnerClientId);
            }

            if (AllReady)
            {
                GameManager.instance?.PlayGame();
            }
        }
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
            GameManager.instance?.PlayGame();
		}
	}
}
