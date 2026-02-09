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
    private Quaternion rotation;

    public static event Action<Quaternion> OnCheatCode;

    // Update is called once per frame
    void Update()
    {
        if (!Input.anyKeyDown) return;

        if (Input.GetKeyDown(cheatcode[index]))
        {
            index++;

            if (index >= cheatcode.Length)
            {
                rotation.SetLookRotation(transform.position);
                OnCheatCode?.Invoke(rotation);
                index = 0;
            }
        }
        else
        {
            index = 0;
        }
    }
}
