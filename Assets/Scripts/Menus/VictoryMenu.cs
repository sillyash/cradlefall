using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;
using NUnit.Framework.Internal;

public class VictoryMenu : MonoBehaviour
{
    [Header("Sound FX Manager")]
    [SerializeField] private SoundFXManager soundFXManager;

    [Header("Audio")]
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] [Range(0f, 1f)] private float buttonClickVolume = 1f;
    [SerializeField] private AudioClip buttonHoverSound;
    [SerializeField] [Range(0f, 1f)] private float buttonHoverVolume = 1f;

    private void PlayButtonSound()
    {
        if (buttonClickSound != null && soundFXManager != null)
        {
            soundFXManager.PlaySoundFXClip(buttonClickSound, transform, buttonClickVolume, true);
        }
    }

    public void OnButtonHover()
    {
        if (buttonHoverSound != null && soundFXManager != null)
        {
            soundFXManager.PlaySoundFXClip(buttonHoverSound, transform, buttonHoverVolume);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void onButtonReturnToMenuClicked()
    {
        PlayButtonSound();
        Debug.Log("Returning to Main Menu...");
        GameManager.instance.ReturnToMainMenu();
    }
}
