using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class DeathCamera : NetworkBehaviour
{
    public float yaw = 6;
    public float tilt = 15;

    private float duration = 1f;
    public AnimationCurve curve;
    public RectTransform iris;

    public override void OnNetworkSpawn()
    {
        gameObject.SetActive(IsOwner);
        if (!IsOwner) return;
        KeyMappingSettings.LoadPreset();
        StartCoroutine(OpenIris());
    }

    IEnumerator OpenIris()
    {
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float value = curve.Evaluate(t / duration);
            iris.localScale = new Vector3(value, value, 1);
            yield return null;
        }

        iris.localScale = Vector3.zero;
        iris.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!IsOwner) return;
        HandleLook();
    }

    void HandleLook()
    {
        float axisX = CustomInputRouter.GetAxisHorizontal();
        float axisY = CustomInputRouter.GetAxisVertical();

        yaw += axisX * 30 * Time.deltaTime;
        tilt += axisY * 30 * Time.deltaTime;

        yaw = Mathf.Clamp(yaw, -60f, 60f);
        tilt = Mathf.Clamp(tilt, -40f, 40f);

        transform.rotation = Quaternion.Euler(-tilt, yaw, 0f);
    }
}
