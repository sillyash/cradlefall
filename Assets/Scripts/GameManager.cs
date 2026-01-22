using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using Constants;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance;
    public enum GameState { MainMenu, Initialising, Playing, Paused, GameOver, Victory}

    private NetworkVariable<GameState> currentState = new(
        GameState.MainMenu,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;
        base.OnNetworkSpawn();

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

    public void ChangeState( GameState newState)
    {
        if (!IsServer) return;
        currentState.Value = newState;
    }

    /// <summary>
    /// [Server only] Loads a game scene. (multiplayer only)
    /// </summary>
    /// <param name="scene">Scene to load. Must be a membre of Constants.Scenes</param>
    /// <exception cref="ArgumentException">The scene is not a valid scene</exception>
    public void ChangeScene(string scene)
    {
        if (!IsServer) return;
        if (!Scenes.isValidScene(scene)) throw new ArgumentException($"Scene : \"{scene}\" is not a valid scene. Please see Constants/Scenes.");
        NetworkManager.Singleton.SceneManager.LoadScene(
            scene,
            LoadSceneMode.Single
        );
    }

    public void HostGame()
    {
        NetworkManager.Singleton.StartHost();
        ChangeState(GameState.Playing);
        ChangeScene(Scenes.MainGameMulti);
    }

    public void PauseGame()
    {
        ChangeState(GameState.Paused);
    }

    public void ResumeGame()
    {
        ChangeState(GameState.Playing);

    }

    public void GameOver()
    {
        ChangeState(GameState.GameOver);
        ChangeScene(Scenes.LoseMenu);
        Cursor.lockState = CursorLockMode.None;
    }

    public void Victory()
    {
        ChangeState(GameState.Victory);
        ChangeScene(Scenes.VictoryMenu);
        Cursor.lockState = CursorLockMode.None;
    }

    public void ReturnToMainMenu()
    {
        ChangeState(GameState.MainMenu);
        ChangeScene(Scenes.MainMenu);
        Cursor.lockState = CursorLockMode.None;
    }

    public void GoToMultiScreen()
    {
        ChangeScene(Scenes.MultiScreen);
        Cursor.lockState = CursorLockMode.None;
    }

    [Obsolete("StartGame should probably not be used. Use Multiplayer startup instead")]
    public void StartGame()
    {
        Debug.Log("StartGame");
        ChangeState(GameState.Initialising);
        SceneManager.LoadScene("MainGame");
    }
}
