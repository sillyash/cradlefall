using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private bool lockXRotation = false;
    [SerializeField] private Camera explicitCamera;

    private Transform targetCameraTransform;
    private float nextResolveAt = 0f;

    public void SetLockXRotation(bool value)
    {
        lockXRotation = value;
    }

    public void SetTargetCamera(Camera camera)
    {
        explicitCamera = camera;
        targetCameraTransform = explicitCamera != null ? explicitCamera.transform : null;
    }

    void LateUpdate()
    {
        if (targetCameraTransform == null || !targetCameraTransform.gameObject.activeInHierarchy)
        {
            ResolveViewerCamera();
        }

        if (targetCameraTransform == null)
            return;

        Vector3 toCamera = targetCameraTransform.position - transform.position;
        if (toCamera.sqrMagnitude < 0.0001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(toCamera.normalized);
        if (lockXRotation)
        {
            Vector3 euler = targetRotation.eulerAngles;
            targetRotation = Quaternion.Euler(0f, euler.y, 0f);
        }

        transform.rotation = targetRotation;
    }

    private void ResolveViewerCamera()
    {
        if (Time.unscaledTime < nextResolveAt)
            return;

        nextResolveAt = Time.unscaledTime + 0.5f;

        if (explicitCamera != null && explicitCamera.isActiveAndEnabled)
        {
            targetCameraTransform = explicitCamera.transform;
            return;
        }

        basePlayer[] players = FindObjectsByType<basePlayer>(FindObjectsSortMode.None);
        foreach (basePlayer player in players)
        {
            if (player == null || !player.IsOwner)
                continue;

            Camera ownerCamera = player.GetComponentInChildren<Camera>(true);
            if (ownerCamera != null && ownerCamera.isActiveAndEnabled)
            {
                targetCameraTransform = ownerCamera.transform;
                return;
            }
        }

        if (Camera.main != null && Camera.main.isActiveAndEnabled)
            targetCameraTransform = Camera.main.transform;
    }
}
