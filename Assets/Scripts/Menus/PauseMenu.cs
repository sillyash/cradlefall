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
            Debug.Log("[PauseMenuHandler] Pause key pressed. Toggling pause menu.");

            if ( ! this.pauseMenu.activeSelf)
            {
                Debug.Log("[PauseMenuHandler] Pause menu is not active. Pausing game.");
                PauseGame();
                return;
            }

            if ( ! IsSettingsPanelActive())
            {
                Debug.Log("[PauseMenuHandler] Pause menu is active. Resuming game.");
                ResumeGame();
                return;
            }
        }
    }

    private GameObject GetSettingsPanel()
    {
        Transform settingsPanelTransform = pauseMenu.transform.Find("Canvas/SettingsPanel");
        if (settingsPanelTransform == null)
        {
            Debug.LogError("[PauseMenu] SettingsPanel not found! Check hierarchy: pauseMenu > Canvas > SettingsPanel");
            return null;
        }
        return settingsPanelTransform.gameObject;
    }

    private bool IsSettingsPanelActive()
    {
        return GetSettingsPanel().activeSelf;
    }

    private void PauseGame()
    {
        this.pauseMenu.SetActive(true);
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void ResumeGame()
    {
        this.pauseMenu.SetActive(false);
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnResumeButtonClicked()
    {
        ResumeGame();
    }

    public void OnSettingsButtonClicked()
    {
        GetSettingsPanel().SetActive(true);
    }

    public void OnQuitToMainMenuButtonClicked()
    {
        isPaused = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameManager.instance.ReturnToMainMenu();
    }
}
