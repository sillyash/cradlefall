using UnityEngine;

public enum PresetType
{
    ZQSD,
    WASD
}

public static class KeyMappingSettings
{
    public static KeyCode Forward;
    public static KeyCode Backward;
    public static KeyCode Left;
    public static KeyCode Right;
    public static KeyCode Jump;
    public static KeyCode Shoot;

    private const string PREFS_KEY = "CurrentPreset";

    // Applique le preset ZQSD classique
    public static void ApplyPresetZQSD()
    {
        Forward = KeyCode.Z;
        Backward = KeyCode.S;
        Left = KeyCode.Q;
        Right = KeyCode.D;
        Jump = KeyCode.Space;
        Shoot = KeyCode.Mouse0;

        PlayerPrefs.SetString(PREFS_KEY, PresetType.ZQSD.ToString());
        PlayerPrefs.Save();
    }

    // Applique le preset WASD classique
    public static void ApplyPresetWASD()
    {
        Forward = KeyCode.W;
        Backward = KeyCode.S;
        Left = KeyCode.A;
        Right = KeyCode.D;
        Jump = KeyCode.Space;
        Shoot = KeyCode.Mouse0;

        PlayerPrefs.SetString(PREFS_KEY, PresetType.WASD.ToString());
        PlayerPrefs.Save();
    }

    // Charge le preset sauvegardé ou applique ZQSD par défaut
    public static void LoadPreset()
    {
        string preset = PlayerPrefs.GetString(PREFS_KEY, PresetType.ZQSD.ToString());
        if (preset == PresetType.WASD.ToString())
        {
            ApplyPresetWASD();
        }
        else
        {
            ApplyPresetZQSD();
        }
    }
}
