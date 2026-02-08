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

    private int index = 0;

    // Update is called once per frame
    void Update()
    {
        if (!Input.anyKeyDown) return;

        if (Input.GetKeyDown(cheatcode[index]))
        {
            index++;

            if (index >= cheatcode.Length)
            {
                OnCheatCode();
                index = 0;
            }
        }
        else
        {
            // Only reset if the player pressed a key that is part of the konami set
            index = 0;
        }
    }

    void OnCheatCode()
    {
        Debug.Log("THRILLER");
    }
}
