using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using NUnit.Framework.Internal;

public class MainMenu : MonoBehaviour {
    [SerializeField] private IPPanelHandler ipPanelHandler;

    private void Awake()
    {
        ResolveIPPanelHandler();
    }

    private void ResolveIPPanelHandler()
    {
        if (ipPanelHandler != null)
            return;

        ipPanelHandler = FindObjectOfType<IPPanelHandler>();
        if (ipPanelHandler != null)
            return;

        // Fallback: also search inactive objects that are part of a loaded scene.
        ipPanelHandler = Resources
            .FindObjectsOfTypeAll<IPPanelHandler>()
            .FirstOrDefault(handler => handler.gameObject.scene.IsValid());
    }

    public void OnHostButtonClicked ()
    {
        GameManager.instance.HostGame();
    }

    public void OnJoinButtonClicked()
    {
        ResolveIPPanelHandler();

        if (ipPanelHandler == null)
        {
            Debug.LogError("[MainMenu] IPPanelHandler not found in scene.");
            return;
        }

        ipPanelHandler.DisplayPanel();
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
