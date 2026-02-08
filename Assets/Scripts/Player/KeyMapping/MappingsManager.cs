
using UnityEngine;
using UnityEngine.UI;

public class InputRebindingUI : MonoBehaviour
{
    [Header("Preset Buttons")]
    [SerializeField] private Button ZQSDPresetButton;
    [SerializeField] private Button WASDPresetButton;

    [Header("Custom Key Buttons")]
    [SerializeField] private Button ForwardKeyButton;
    [SerializeField] private Button BackwardKeyButton;
    [SerializeField] private Button LeftKeyButton;
    [SerializeField] private Button RightKeyButton;
    [SerializeField] private Button JumpKeyButton;
    [SerializeField] private Button ShootKeyButton;
    [SerializeField] private Button ReloadKeyButton;
    [SerializeField] private Button InteractKeyButton;

    void Start()
    {
        ZQSDPresetButton.onClick.AddListener(() => {
            KeyMappingSettings.ApplyPresetZQSD();
        });

        WASDPresetButton.onClick.AddListener(() => {
            KeyMappingSettings.ApplyPresetWASD();
        });
    }
}
