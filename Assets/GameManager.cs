using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public enum GameState { MainMenu, Playing, Paused, GameOver, Victory}
    public GameState currentState;

    public void ChangeState( GameState newState)
    {
        currentState = newState;
    }

    public void StartGame()
    {
        ChangeState(GameState.Playing);
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
        Debug.Log("You just lost you loser!");
        ReturnToMainMenu();
    }

    public void Victory()
    {
        ChangeState(GameState.Victory);
    }

    public void ReturnToMainMenu()
    {
        ChangeState(GameState.MainMenu);
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

