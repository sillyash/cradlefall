using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused { get; private set; } = false;

    [Header("Sound FX Manager")]
    [SerializeField] private SoundFXManager soundFXManager;

    [Header("Audio")]
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] [Range(0f, 1f)] private float buttonClickVolume = 1f;
    [SerializeField] private AudioClip buttonHoverSound;
    [SerializeField] [Range(0f, 1f)] private float buttonHoverVolume = 1f;

    [SerializeField] private GameObject pauseMenu;

    private void PlayButtonSound(bool persistOnSceneChange = false)
    {
        if (buttonClickSound != null && soundFXManager != null)
        {
            soundFXManager.PlaySoundFXClip(buttonClickSound, transform, buttonClickVolume, persistOnSceneChange);
        }
    }

    public void OnButtonHover()
    {
        if (buttonHoverSound != null && soundFXManager != null)
        {
            soundFXManager.PlaySoundFXClip(buttonHoverSound, transform, buttonHoverVolume);
        }
    }

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
        PlayButtonSound();
        ResumeGame();
    }

    public void OnSettingsButtonClicked()
    {
        PlayButtonSound();
        GetSettingsPanel().SetActive(true);
    }

    public void OnQuitToMainMenuButtonClicked()
    {
        PlayButtonSound(true);
        isPaused = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameManager.instance.ReturnToMainMenu();
    }
}
