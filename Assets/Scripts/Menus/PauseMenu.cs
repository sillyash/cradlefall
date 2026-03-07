using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused { get; private set; } = false;
    
    [SerializeField] private GameObject pauseMenu;

    void Awake()
    {
        this.pauseMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("Tab key pressed. Toggling pause menu.");
            if (this.pauseMenu.activeSelf)
            {
                Debug.Log("Pause menu is active. Resuming game.");
                OnResumeButtonClicked();
            }
            else
            {
                Debug.Log("Pause menu is not active. Pausing game.");
                this.pauseMenu.SetActive(true);
                isPaused = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    public void OnResumeButtonClicked()
    {
        this.pauseMenu.SetActive(false);
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnSettingsButtonClicked()
    {
        // TODO: Implement settings menu
        Debug.Log("Settings button clicked. This feature is not implemented yet.");
    }

    public void OnQuitToMainMenuButtonClicked()
    {
        isPaused = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameManager.instance.ReturnToMainMenu();
    }
}
