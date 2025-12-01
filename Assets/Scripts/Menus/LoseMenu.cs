using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;
using NUnit.Framework.Internal;

public class LoseMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void onButtonReturnToMenuClicked()
    {
        Debug.Log("Returning to Main Menu...");
        GameManager.instance.ReturnToMainMenu();
    }
}
