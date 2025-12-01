using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public enum GameState { MainMenu, Initialising, Playing, Paused, GameOver, Victory}
    public GameState currentState;

    public void ChangeState( GameState newState)
    {
        currentState = newState;
    }

    public void StartGame()
    {
        Debug.Log("in StartGame");
        ChangeState(GameState.Initialising);
        SceneManager.LoadScene("VictoryScene");

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
    }

    public void Victory()
    {
        Debug.Log("in GameVictory");
        ChangeState(GameState.Victory);
    }

    public void ReturnToMainMenu()
    {
        Debug.Log("in ReturnToMainMenu");
        ChangeState(GameState.MainMenu);
        SceneManager.LoadScene("MenuScene");
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

