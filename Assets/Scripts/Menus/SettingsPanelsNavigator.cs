using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelsNavigator : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject audioSettingsPanel;
    [SerializeField] private GameObject mappingsSettingsPanel;

    [Header("Tab Buttons")]
    [SerializeField] private Button audioTabButton;
    [SerializeField] private Button mappingsTabButton;

    [Header("Defaults")]
    [SerializeField] private bool showAudioOnStart = true;

    private void Awake()
    {
        if (audioTabButton != null)
            audioTabButton.onClick.AddListener(ShowAudio);

        if (mappingsTabButton != null)
            mappingsTabButton.onClick.AddListener(ShowMappings);
    }

    private void Start()
    {
        if (showAudioOnStart)
            ShowAudio();
        else
            ShowMappings();
    }

    private void OnDestroy()
    {
        if (audioTabButton != null)
            audioTabButton.onClick.RemoveListener(ShowAudio);

        if (mappingsTabButton != null)
            mappingsTabButton.onClick.RemoveListener(ShowMappings);
    }

    public void ShowAudio()
    {
        SetActivePanel(audioSettingsPanel, true);
        SetActivePanel(mappingsSettingsPanel, false);
        SetTabInteractable(audioTabButton, false);
        SetTabInteractable(mappingsTabButton, true);
    }

    public void ShowMappings()
    {
        SetActivePanel(audioSettingsPanel, false);
        SetActivePanel(mappingsSettingsPanel, true);
        SetTabInteractable(audioTabButton, true);
        SetTabInteractable(mappingsTabButton, false);
    }

    private static void SetActivePanel(GameObject panel, bool isActive)
    {
        if (panel != null)
            panel.SetActive(isActive);
    }

    private static void SetTabInteractable(Button button, bool isInteractable)
    {
        if (button != null)
            button.interactable = isInteractable;
    }
}
