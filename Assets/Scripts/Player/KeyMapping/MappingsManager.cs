
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MappingsManager : MonoBehaviour
{
    [Header("Preset Buttons")]
    [SerializeField] private Button ZQSDPresetButton;
    [SerializeField] private Button WASDPresetButton;
    [SerializeField] private Button resetToDefaultButton;

    [Header("Key Binding Buttons")]
    [SerializeField] private KeyBindingButton forwardKeyButton;
    [SerializeField] private KeyBindingButton backwardKeyButton;
    [SerializeField] private KeyBindingButton leftKeyButton;
    [SerializeField] private KeyBindingButton rightKeyButton;
    [SerializeField] private KeyBindingButton jumpKeyButton;
    [SerializeField] private KeyBindingButton shootKeyButton;
    [SerializeField] private KeyBindingButton reloadKeyButton;
    [SerializeField] private KeyBindingButton interactKeyButton;

    private KeyBindingButton currentlyRebinding = null;
    private KeyMappingAction currentAction;
    private List<KeyBindingButton> allKeyButtons = new List<KeyBindingButton>();

    void Start()
    {
        // Initialize all key binding buttons
        InitializeKeyBindingButton(forwardKeyButton, KeyMappingAction.Forward);
        InitializeKeyBindingButton(backwardKeyButton, KeyMappingAction.Backward);
        InitializeKeyBindingButton(leftKeyButton, KeyMappingAction.Left);
        InitializeKeyBindingButton(rightKeyButton, KeyMappingAction.Right);
        InitializeKeyBindingButton(jumpKeyButton, KeyMappingAction.Jump);
        InitializeKeyBindingButton(shootKeyButton, KeyMappingAction.Shoot);
        InitializeKeyBindingButton(reloadKeyButton, KeyMappingAction.Reload);
        InitializeKeyBindingButton(interactKeyButton, KeyMappingAction.Interact);

        // Setup preset buttons
        ZQSDPresetButton.onClick.AddListener(() => {
            KeyMappingSettings.ApplyPresetZQSD();
            RefreshAllKeyDisplays();
        });

        WASDPresetButton.onClick.AddListener(() => {
            KeyMappingSettings.ApplyPresetWASD();
            RefreshAllKeyDisplays();
        });

        if (resetToDefaultButton != null)
        {
            resetToDefaultButton.onClick.AddListener(() => {
                KeyMappingSettings.ApplyPresetFromLocale();
                RefreshAllKeyDisplays();
            });
        }

        // Ensure all key displays are updated after UI is fully initialized
        RefreshAllKeyDisplays();
    }

    void Update()
    {
        // If we're waiting for a key press
        if (currentlyRebinding != null)
        {
            // Check for any key press
            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    // Ignore escape key (cancel rebinding)
                    if (keyCode == KeyCode.Escape)
                    {
                        CancelRebinding();
                        return;
                    }

                    // Apply the new key binding
                    KeyMappingSettings.SetCustomKey(currentAction, keyCode);
                    currentlyRebinding.UpdateKeyDisplay();
                    currentlyRebinding.SetWaitingState(false);
                    currentlyRebinding = null;
                    
                    Debug.Log($"Rebound {currentAction} to {keyCode}");
                    return;
                }
            }
        }
    }

    private void InitializeKeyBindingButton(KeyBindingButton button, KeyMappingAction action)
    {
        if (button != null)
        {
            button.Initialize(action, OnRebindRequested);
            allKeyButtons.Add(button);
        }
        else
        {
            Debug.LogWarning($"[MappingsManager] KeyBindingButton for {action} is null! Please assign it in the inspector.");
        }
    }

    private void OnRebindRequested(KeyMappingAction action, KeyBindingButton button)
    {
        // Cancel any current rebinding
        if (currentlyRebinding != null)
        {
            currentlyRebinding.SetWaitingState(false);
        }

        // Start waiting for new key
        currentAction = action;
        currentlyRebinding = button;
        button.SetWaitingState(true);
        
        Debug.Log($"Waiting for key press for action: {action}");
    }

    private void CancelRebinding()
    {
        if (currentlyRebinding != null)
        {
            currentlyRebinding.SetWaitingState(false);
            currentlyRebinding = null;
        }
    }

    private void RefreshAllKeyDisplays()
    {
        foreach (var button in allKeyButtons)
        {
            if (button != null)
            {
                button.UpdateKeyDisplay();
            }
        }
    }

    void OnDisable()
    {
        CancelRebinding();
    }
}
