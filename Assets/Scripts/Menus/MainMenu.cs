using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;
using NUnit.Framework.Internal;

public class MainMenu : MonoBehaviour 
{
    
    [Header("Sound FX Manager")]
    [SerializeField] private SoundFXManager soundFXManager;

    [SerializeField] private AudioSource menuMusicSource;
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] [Range(0f,1f)] private float buttonClickVolume = 1f;
    [SerializeField] private AudioClip buttonHoverSound;
    [SerializeField] [Range(0f,1f)] private float buttonHoverVolume = 1f;

    void Start()
    {
        if (menuMusicSource != null)
        {
            menuMusicSource.Play();
        }
    }

    private void PlayButtonSound()
    {
        if (buttonClickSound != null && soundFXManager != null)
        {
            soundFXManager.PlaySoundFXClip(buttonClickSound, transform, buttonClickVolume, true);
        }
    }

    public void OnHostButtonClicked ()
    {
        PlayButtonSound();
        GameManager.instance.HostGame();
    }

    public void OnJoinButtonClicked()
    {
        PlayButtonSound();
        GameManager.instance.StartClient();
    }

    public void OnQuitButtonClicked()
    {
        PlayButtonSound();
        Application.Quit();
    }

    public void OnOptionsButtonClicked()
    {
        PlayButtonSound();
        SceneManager.LoadScene("SettingsMenu");
    }

    public void onCreditsButtonClicked()
    {
        PlayButtonSound();
        Debug.Log("Loading Credits Menu...");
        // TODO: Load Credits Menu
    }

    public void OnButtonHover()
    {
        if (buttonHoverSound != null && soundFXManager != null)
        {
            soundFXManager.PlaySoundFXClip(buttonHoverSound, transform, buttonHoverVolume);
        }
    }
}
