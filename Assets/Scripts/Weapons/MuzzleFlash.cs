using UnityEngine;
using System.Collections;

public class MuzzleFlash : MonoBehaviour
{
    public Light muzzleLight;
    public float flashDuration = 0.05f;

    public void Start()
    {
        muzzleLight.enabled = false;
    }

    public void PlayMuzzleFlash()
    {
        StartCoroutine(Flash());
    }

    private IEnumerator Flash()
    {
        muzzleLight.enabled = true;
        yield return new WaitForSeconds(flashDuration);
        muzzleLight.enabled = false;
    }
}
