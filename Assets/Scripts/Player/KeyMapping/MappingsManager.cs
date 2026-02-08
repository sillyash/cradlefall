
using UnityEngine;
using UnityEngine.UI;

public class InputRebindingUI : MonoBehaviour
{
    public Button ZQSDPresetButton;
    public Button WASDPresetButton;

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
