using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuPrefab;
    private GameObject pauseMenuInstance;

    void Awake()
    {
        this.pauseMenuInstance = Instantiate(pauseMenuPrefab);
        this.pauseMenuInstance.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("Tab key pressed. Toggling pause menu.");
            if (this.pauseMenuInstance.activeSelf)
            {
                Debug.Log("Pause menu is active. Resuming game.");
                OnResumeButtonClicked();
            }
            else
            {
                Debug.Log("Pause menu is not active. Pausing game.");
                this.pauseMenuInstance.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    public void OnResumeButtonClicked()
    {
        this.pauseMenuInstance.SetActive(false);
    }

    public void OnQuitToMainMenuButtonClicked()
    {
        GameManager.instance.ReturnToMainMenu();
    }
}
