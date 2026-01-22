using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using Constants;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance;
    public enum GameState { MainMenu, Initialising, Playing, Paused, GameOver, Victory, MultiScreen}
    public GameState currentState;

    public override void OnNetworkSpawn()
    {
        if (!IsServer) return;
        base.OnNetworkSpawn();
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeState( GameState newState)
    {
        currentState = newState;
    }

    public void StartGame()
    {
        Debug.Log("StartGame");
        ChangeState(GameState.Initialising);
        SceneManager.LoadScene("MainGame");

    }

    public void PauseGame()
    {
        Debug.Log("in PauseGame");
        ChangeState(GameState.Paused);
    }

    public void ResumeGame()
    {
        Debug.Log("in ResumeGame");
        ChangeState(GameState.Playing);

    }

    public void GameOver()
    {
        Debug.Log("in GameOver");
        ChangeState(GameState.GameOver);
        SceneManager.LoadScene("LoseMenu");
        Cursor.lockState = CursorLockMode.None;
    }

    public void Victory()
    {
        Debug.Log("in GameVictory");
        ChangeState(GameState.Victory);
        SceneManager.LoadScene("VictoryMenu");
        Cursor.lockState = CursorLockMode.None;
    }

    public void ReturnToMainMenu()
    {
        Debug.Log("in ReturnToMainMenu");
        ChangeState(GameState.MainMenu);
        SceneManager.LoadScene("MainMenu");
        Cursor.lockState = CursorLockMode.None;
    }

    public void GoToMultiScreen()
    {
        ChangeState(GameState.MultiScreen);
        SceneManager.LoadScene("MultiScreen");
        Cursor.lockState = CursorLockMode.None;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }       
}
