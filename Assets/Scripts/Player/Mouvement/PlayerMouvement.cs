using UnityEngine;

public abstract class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    protected PlayerMovementSettings movementSettings; // ScriptableObject à assigner dans l’inspecteur

    protected virtual void Awake()
    {
        if (movementSettings == null)
            Debug.LogError($"{GetType().Name} requires a PlayerDataMovement ScriptableObject!");
    }

    protected abstract void HandleMovement();
    protected abstract void HandleJump();
    protected abstract void ApplyGravity();
    protected abstract void ApplyFinalMovement();
}