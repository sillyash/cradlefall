using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using Constants;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance;
    [SerializeField] private GameObject ProjectileMachine;

    public NetworkVariable<GameState> currentState = new(
        GameState.Playing,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

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

    /// <summary>
    /// Loads a game scene and changes GameState accordingly.
    /// </summary>
    /// <param name="scene">Scene to load. Must be a membre of Constants.Scenes</param>
    /// <exception cref="ArgumentException">The scene is not a valid scene</exception>
    public void ChangeScene(string scene)
    {
        if (!Scenes.isValidScene(scene) || !Scenes.SceneToStateMap.TryGetValue(scene, out GameState newState))
            throw new ArgumentException($"Scene : \"{scene}\" is not a valid scene. Please see Constants/Scenes.");
        
        if (!Scenes.isSoloScene(scene))
        {
            // Multiplayer
            if (!IsServer) return;

            currentState.Value = newState;
            NetworkManager.Singleton.SceneManager.LoadScene(
                scene,
                LoadSceneMode.Single
            );
        } 
        else
        {
            // Solo (menus)
            if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsClient)
            {
                // Disconects Client if needed
                NetworkManager.Singleton.Shutdown();
            }
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }
    }

    public void HostGame()
    {
        NetworkManager.Singleton.OnServerStarted += OnServerStarted;
        NetworkManager.Singleton.StartHost();
    }

    private void OnServerStarted()
    {
        NetworkManager.Singleton.OnServerStarted -= OnServerStarted;
        // Load lobby scene instead of going directly to game
        NetworkManager.Singleton.SceneManager.LoadScene(
            Scenes.Lobby,
            LoadSceneMode.Single
        );
        var obj = Instantiate(ProjectileMachine);
        obj.GetComponent<NetworkObject>().Spawn();
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    public void PauseGame()
    {
        throw new NotImplementedException(); // TODO
    }

    public void ResumeGame()
    {
        throw new NotImplementedException(); // TODO
    }

    public void GameOver()
    {
        GameOverRpc();
    }

    /// <summary>
    /// Notify server of game over and change scene for all clients
    /// </summary>
    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    public void GameOverRpc()
    {
        // Despawn all players before changing scene
        if (NetworkManager.Singleton.IsServer && PlayerManager.Instance != null)
        {
            foreach (var player in PlayerManager.Instance.GetAllPlayers())
            {
                PlayerManager.Instance.DespawnPlayerForClient(player.getClientId());
            }
        }
        
        ChangeScene(Scenes.LoseMenu);
        Cursor.lockState = CursorLockMode.None;
    }

    public void Victory()
    {
        VictoryRpc();
    }

    /// <summary>
    /// Notify server of victory and change scene for all clients
    /// </summary>
    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    public void VictoryRpc()
    {
        ChangeScene(Scenes.VictoryMenu);
        Cursor.lockState = CursorLockMode.None;
    }

    /// <summary>
    /// Return to main menu from any scene
    /// </summary>
    public void ReturnToMainMenu()
    {
        ChangeScene(Scenes.MainMenu);
        Cursor.lockState = CursorLockMode.None;
    }

    /// <summary>
    /// Go to multiplayer screen from main menu
    /// </summary>
    public void GoToMultiScreen()
    {
        ChangeScene(Scenes.MultiScreen);
        Cursor.lockState = CursorLockMode.None;
    }

    /// <summary>
    /// StartGame should probably not be used. Use Multiplayer startup instead
    /// </summary>
    public void StartGame()
    {
        Debug.Log("[GameManager] StartGame");
        //ChangeState(GameState.Initialising);
        SceneManager.LoadScene("MainGame");
    }
}
