using Unity.Netcode;
using UnityEngine;

public class DeathCamera : NetworkBehaviour
{
    public float yaw = 6;
    public float tilt = 15;
    private void Start()
    {
        if (!IsOwner) return;
        KeyMappingSettings.ApplyPresetZQSD();
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
