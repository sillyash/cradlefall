using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;

public class MainMenu : MonoBehaviour {

    public void OnPlayButtonClicked ()
    {
        Debug.Log("Playing game");
        GameManager.instance.StartGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    public void OnQuitButtonClicked()
    {
        Debug.Log("Quitting now.");
        GameManager.instance.GameOver();
        Application.Quit();
    }

    public void OnOptionsButtonClicked()
    {
        Debug.Log("Options not implemented yet");
    }
}
