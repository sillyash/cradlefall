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
        SceneManager.LoadScene("Options");
    }

}
