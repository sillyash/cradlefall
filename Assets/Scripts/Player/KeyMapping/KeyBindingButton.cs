using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class KeyBindingButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionNameText;
    [SerializeField] private TextMeshProUGUI keyNameText;
    [SerializeField] private Button rebindButton;
    
    [SerializeField] private KeyMappingAction action;
    private Action<KeyMappingAction, KeyBindingButton> onRebindRequested;
    private bool isWaitingForKey = false;

    void Awake()
    {
        // Validate references
        if (actionNameText == null)
            Debug.LogError($"[KeyBindingButton] actionNameText not assigned on {gameObject.name}");
        if (keyNameText == null)
            Debug.LogError($"[KeyBindingButton] keyNameText not assigned on {gameObject.name}");
        if (rebindButton == null)
            Debug.LogError($"[KeyBindingButton] rebindButton not assigned on {gameObject.name}");
    }

    public void Initialize(KeyMappingAction action, Action<KeyMappingAction, KeyBindingButton> onRebindCallback)
    {
        this.action = action;
        this.onRebindRequested = onRebindCallback;
        
        if (actionNameText == null)
        {
            Debug.LogError($"[KeyBindingButton] actionNameText is null on {gameObject.name}!");
            return;
        }
        
        if (keyNameText == null)
        {
            Debug.LogError($"[KeyBindingButton] keyNameText is null on {gameObject.name}!");
            return;
        }
        
        if (rebindButton == null)
        {
            Debug.LogError($"[KeyBindingButton] rebindButton is null on {gameObject.name}!");
            return;
        }
        
        actionNameText.text = GetActionDisplayName(action);
        UpdateKeyDisplay();
        
        rebindButton.onClick.AddListener(OnRebindButtonClicked);
    }

    public void UpdateKeyDisplay()
    {
        KeyCode currentKey = GetCurrentKeyForAction(action);
        keyNameText.text = GetKeyDisplayName(currentKey);
    }

    public void SetWaitingState(bool waiting)
    {
        isWaitingForKey = waiting;
        keyNameText.text = waiting ? "Press any key..." : GetKeyDisplayName(GetCurrentKeyForAction(action));
        keyNameText.color = waiting ? Color.yellow : Color.white;
    }

    private void OnRebindButtonClicked()
    {
        onRebindRequested?.Invoke(action, this);
    }

    private string GetActionDisplayName(KeyMappingAction action)
    {
        switch (action)
        {
            case KeyMappingAction.Forward: return "Move Forward";
            case KeyMappingAction.Backward: return "Move Backward";
            case KeyMappingAction.Left: return "Move Left";
            case KeyMappingAction.Right: return "Move Right";
            case KeyMappingAction.Jump: return "Jump";
            case KeyMappingAction.Shoot: return "Shoot";
            case KeyMappingAction.Reload: return "Reload";
            case KeyMappingAction.Interact: return "Interact";
            default: return action.ToString();
        }
    }

    private KeyCode GetCurrentKeyForAction(KeyMappingAction action)
    {
        switch (action)
        {
            case KeyMappingAction.Forward: return KeyMappingSettings.Forward;
            case KeyMappingAction.Backward: return KeyMappingSettings.Backward;
            case KeyMappingAction.Left: return KeyMappingSettings.Left;
            case KeyMappingAction.Right: return KeyMappingSettings.Right;
            case KeyMappingAction.Jump: return KeyMappingSettings.Jump;
            case KeyMappingAction.Shoot: return KeyMappingSettings.Shoot;
            case KeyMappingAction.Reload: return KeyMappingSettings.Reload;
            case KeyMappingAction.Interact: return KeyMappingSettings.Interact;
            default: return KeyCode.None;
        }
    }

    private string GetKeyDisplayName(KeyCode key)
    {
        // Handle special cases for better display names
        switch (key)
        {
            case KeyCode.Mouse0: return "Left Mouse";
            case KeyCode.Mouse1: return "Right Mouse";
            case KeyCode.Mouse2: return "Middle Mouse";
            case KeyCode.Space: return "Space";
            case KeyCode.LeftShift: return "Left Shift";
            case KeyCode.RightShift: return "Right Shift";
            case KeyCode.LeftControl: return "Left Ctrl";
            case KeyCode.RightControl: return "Right Ctrl";
            case KeyCode.LeftAlt: return "Left Alt";
            case KeyCode.RightAlt: return "Right Alt";
            default: return key.ToString();
        }
    }
}
