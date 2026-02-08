using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsSliders : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MixerVolumeManager volumeManager;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundFXSlider;

    private void Start()
    {
        SyncSlidersFromMixer();
    }

    private void OnEnable()
    {
        SyncSlidersFromMixer();
    }

    public void SyncSlidersFromMixer()
    {
        if (!volumeManager)
        {
            return;
        }

        SetSliderValue(masterSlider, volumeManager.GetMasterVolume());
        SetSliderValue(musicSlider, volumeManager.GetMusicVolume());
        SetSliderValue(soundFXSlider, volumeManager.GetSoundFXVolume());
    }

    private static void SetSliderValue(Slider slider, float value)
    {
        if (!slider)
        {
            return;
        }

        float clamped = Mathf.Clamp(value, slider.minValue, slider.maxValue);
        slider.SetValueWithoutNotify(clamped);
    }
}
