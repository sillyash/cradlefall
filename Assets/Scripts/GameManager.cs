using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using Constants;
using System.Collections.Generic;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance;

    private NetworkVariable<GameState> currentState = new(
        GameState.Menu,
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

    /// <summary>
    /// [Server only] Loads a game scene and chages GameState accordingly. (multiplayer only)
    /// </summary>
    /// <param name="scene">Scene to load. Must be a membre of Constants.Scenes</param>
    /// <exception cref="ArgumentException">The scene is not a valid scene</exception>
    public void ChangeScene(string scene)
    {
        if (!IsServer) return;

        if (Scenes.isValidScene(scene) && Scenes.SceneToStateMap.TryGetValue(scene, out GameState newState))
        {
            currentState.Value = newState;
            NetworkManager.Singleton.SceneManager.LoadScene(
                scene,
                LoadSceneMode.Single
            );
        } else
        {
            throw new ArgumentException($"Scene : \"{scene}\" is not a valid scene. Please see Constants/Scenes.");
        }
    }

    public void HostGame()
    {
        NetworkManager.Singleton.StartHost();
        ChangeScene(Scenes.MainGameMulti);
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
        ChangeScene(Scenes.LoseMenu);
        Cursor.lockState = CursorLockMode.None;
    }

    public void Victory()
    {
        ChangeScene(Scenes.VictoryMenu);
        Cursor.lockState = CursorLockMode.None;
    }

    public void ReturnToMainMenu()
    {
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
        //ChangeState(GameState.Initialising);
        SceneManager.LoadScene("MainGame");
    }
}
