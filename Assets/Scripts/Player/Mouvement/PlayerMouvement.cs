using UnityEngine;
using Unity.Netcode;

public abstract class PlayerMovement : NetworkBehaviour
{
    [SerializeField]
    protected PlayerMovementSettings movementSettings; // ScriptableObject à assigner dans l’inspecteur

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        if (movementSettings == null)
            Debug.LogError($"{GetType().Name} requires a PlayerDataMovement ScriptableObject!");
    }

    protected abstract void HandleMovement();
    protected abstract void HandleJump();
    protected abstract void ApplyGravity();
    protected abstract void ApplyFinalMovement();
}