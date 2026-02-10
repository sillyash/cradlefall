using System;
using UnityEngine;

public class SecretFeature : MonoBehaviour
{
    private KeyCode[] cheatcode = new KeyCode[]
    {
        KeyCode.D,
        KeyCode.A,
        KeyCode.N,
        KeyCode.C,
        KeyCode.E,
    };

    private int index;
    private Quaternion rotation;
    private KeyCode nextCodeKey;
    private KeyCode currentCodeKey;
    public static event Action<Quaternion> OnCheatCode;

    public SecretFeature()
    {
        this.index = 0;
        this.currentCodeKey = KeyCode.None;
        this.nextCodeKey = cheatcode[0];
    }

    // Update is called once per frame
    void Update()
    {
        if ( ! Input.anyKeyDown) return;

        // If we haven't broke the cheat code but are still pressing the same key
        if (currentCodeKey != KeyCode.None && Input.GetKeyDown(currentCodeKey)) return;

        // Check if we're pressing the correct next cheat code key
        bool nextKeyOK = Input.GetKeyDown(nextCodeKey);

        // Cheat code failed
        if ( ! nextKeyOK)
        {
            ResetCheatCode();
            return;
        }

        // Progress
        this.index++;

        // Cheat code done
        if (this.index == cheatcode.Length)
        {
            rotation.SetLookRotation(transform.position);
            OnCheatCode?.Invoke(rotation);

            ResetCheatCode();
            return;
        }

        // Cheat code increment
        this.nextCodeKey = cheatcode[this.index];
        this.currentCodeKey = cheatcode[this.index - 1];
    }

    void ResetCheatCode()
    {
        this.index = 0;
        this.currentCodeKey = KeyCode.None;
        this.nextCodeKey = cheatcode[0];
    }
}
