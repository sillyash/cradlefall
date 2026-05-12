using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class MuzzleFlash : MonoBehaviour
{
    public VisualEffect particle;
    public Light muzzleLight;
    public float flashDuration = 0.05f;

    public void Start()
    {
        muzzleLight.enabled = false;
    }

    public void PlayMuzzleFlash()
    {
        StartCoroutine(Flash());
        particle.Play();
    }

    private IEnumerator Flash()
    {
        muzzleLight.enabled = true;
        yield return new WaitForSeconds(flashDuration);
        muzzleLight.enabled = false;
    }
}
