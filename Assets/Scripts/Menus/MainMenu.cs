using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;
using NUnit.Framework.Internal;

public class MainMenu : MonoBehaviour {

    public void OnPlayButtonClicked ()
    {
        Debug.Log("Playing game");
        GameManager.instance.StartGame();
    }

    public void OnQuitButtonClicked()
    {
        Debug.Log("Quitting now.");
        Application.Quit();
    }

    public void OnOptionsButtonClicked()
    {
        Debug.Log("Loading Options Menu...");
        // TODO: Load Options Menu
    }

    public void onCreditsButtonClicked()
    {
        Debug.Log("Loading Credits Menu...");
        // TODO: Load Credits Menu
    }

    public void OnMultiButtonClicked()
    {
        Debug.Log("Playing game");
        GameManager.instance.GoToMultiScreen();
    }
}
