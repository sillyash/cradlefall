using UnityEngine;

public enum PresetType
{
    ZQSD,
    WASD
}

public enum KeyMappingAction
{
    Forward,
    Backward,
    Left,
    Right,
    Jump,
    Shoot,
    Reload,
    Interact
}

public static class KeyMappingSettings
{
    public static KeyCode Forward;
    public static KeyCode Backward;
    public static KeyCode Left;
    public static KeyCode Right;
    public static KeyCode Jump;
    public static KeyCode Shoot;
    public static KeyCode Reload;
    public static KeyCode Interact;

    private const string PREFS_KEY = "CurrentPreset";
    private const string CUSTOM_KEY_PREFIX = "CustomKey_";

    static KeyMappingSettings()
    {
        LoadPreset();
        LoadCustomKeys();
    }

    // Applique le preset ZQSD classique
    public static void ApplyPresetZQSD()
    {
        Forward = KeyCode.Z;
        Backward = KeyCode.S;
        Left = KeyCode.Q;
        Right = KeyCode.D;
        Jump = KeyCode.Space;
        Shoot = KeyCode.Mouse0;
        Reload = KeyCode.R;
        Interact = KeyCode.E;

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
        Reload = KeyCode.R;
        Interact = KeyCode.E;

        PlayerPrefs.SetString(PREFS_KEY, PresetType.WASD.ToString());
        PlayerPrefs.Save();
    }

    // Charge le preset sauvegardé ou applique ZQSD par défaut
    public static void LoadPreset()
    {
        string preset = PlayerPrefs.GetString(PREFS_KEY, PresetType.ZQSD.ToString());

        if (preset == PresetType.WASD.ToString())
        {
            Debug.Log("[KeyMapping] Detected WASD preset in Player preferences, applying.");
            ApplyPresetWASD();
        }
        else if (preset == PresetType.ZQSD.ToString())
        {
            Debug.Log("[KeyMapping] Detected ZQSD preset in Player preferences, applying.");
            ApplyPresetZQSD();
        }
        else
        {
            Debug.Log("[KeyMapping] No preset detected in Player preferences, applying from locale.");
            ApplyPresetFromLocale();
        }
    }

    // Applique un preset selon la langue système (WASD par défaut)
    public static void ApplyPresetFromLocale()
    {
        switch (Application.systemLanguage)
        {
            case SystemLanguage.French:
                Debug.Log("[KeyMapping] Detected French locale, applying ZQSD preset.");
                ApplyPresetZQSD();
                break;
            default:
                Debug.Log("[KeyMapping] Default locale, applying WASD preset.");
                ApplyPresetWASD();
                break;
        }
    }

    // Charge les touches personnalisées sauvegardées (si présentes)
    public static void LoadCustomKeys()
    {
        ApplyCustomKeyIfExists(KeyMappingAction.Forward);
        ApplyCustomKeyIfExists(KeyMappingAction.Backward);
        ApplyCustomKeyIfExists(KeyMappingAction.Left);
        ApplyCustomKeyIfExists(KeyMappingAction.Right);
        ApplyCustomKeyIfExists(KeyMappingAction.Jump);
        ApplyCustomKeyIfExists(KeyMappingAction.Shoot);
        ApplyCustomKeyIfExists(KeyMappingAction.Reload);
        ApplyCustomKeyIfExists(KeyMappingAction.Interact);
    }

    // Définit une touche personnalisée pour une action donnée
    public static void SetCustomKey(KeyMappingAction action, KeyCode key, bool save = true)
    {
        switch (action)
        {
            case KeyMappingAction.Forward:
                Forward = key;
                break;
            case KeyMappingAction.Backward:
                Backward = key;
                break;
            case KeyMappingAction.Left:
                Left = key;
                break;
            case KeyMappingAction.Right:
                Right = key;
                break;
            case KeyMappingAction.Jump:
                Jump = key;
                break;
            case KeyMappingAction.Shoot:
                Shoot = key;
                break;
            case KeyMappingAction.Reload:
                Reload = key;
                break;
            case KeyMappingAction.Interact:
                Interact = key;
                break;
        }

        if (save)
        {
            PlayerPrefs.SetInt(CUSTOM_KEY_PREFIX + action, (int)key);
            PlayerPrefs.Save();
        }
    }

    private static void ApplyCustomKeyIfExists(KeyMappingAction action)
    {
        string prefsKey = CUSTOM_KEY_PREFIX + action;
        if (!PlayerPrefs.HasKey(prefsKey))
        {
            return;
        }

        KeyCode key = (KeyCode)PlayerPrefs.GetInt(prefsKey);
        SetCustomKey(action, key, save: false);
    }
}
