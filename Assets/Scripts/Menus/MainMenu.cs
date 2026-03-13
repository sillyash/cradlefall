using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;
using NUnit.Framework.Internal;

public class MainMenu : MonoBehaviour {

    [SerializeField] private AudioSource menuMusicSource;

    void Start()
    {
        if (menuMusicSource != null)
        {
            menuMusicSource.Play();
        }
    }

    public void OnHostButtonClicked ()
    {
        GameManager.instance.HostGame();
    }

    public void OnJoinButtonClicked()
    {
        GameManager.instance.StartClient();
    }

    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }

    public void OnOptionsButtonClicked()
    {
        SceneManager.LoadScene("SettingsMenu");
    }

    public void onCreditsButtonClicked()
    {
        Debug.Log("Loading Credits Menu...");
        // TODO: Load Credits Menu
    }
}
