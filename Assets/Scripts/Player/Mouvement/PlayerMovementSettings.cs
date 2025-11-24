using UnityEngine;

/// <summary>
/// Valeurs de base pour le mouvement des Joueurs à personnaliser en fonction des rôles.
/// </summary>
[CreateAssetMenu(fileName = "PlayerMovementSettings", menuName = "Player/Mouvements")]
public class PlayerMovementSettings : ScriptableObject
{
    [Tooltip("Vitesse de marche.")]
    public float walkingSpeed = 10f;

    [Tooltip("Vitesse de course.")]
    public float runningSpeed = 15f;

    [Tooltip("Hauteur de saut.")]
    public float jumpHeight = 2f;

    public const float GRAVITY = -9.81f;
}
