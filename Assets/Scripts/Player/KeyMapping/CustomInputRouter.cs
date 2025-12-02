using UnityEngine;

public static class CustomInputRouter
{
    // Retourne -1, 0, +1 selon les touches appuyées pour l'axe horizontal
    public static float GetAxisHorizontal()
    {
        float axis = 0f;
        if (Input.GetKey(KeyMappingSettings.Left))
            axis -= 1f;
        if (Input.GetKey(KeyMappingSettings.Right))
            axis += 1f;
        return Mathf.Clamp(axis, -1f, 1f);
    }

    // Même pour l’axe vertical (avant/arrière)
    public static float GetAxisVertical()
    {
        float axis = 0f;
        if (Input.GetKey(KeyMappingSettings.Forward))
            axis += 1f;
        if (Input.GetKey(KeyMappingSettings.Backward))
            axis -= 1f;
        return Mathf.Clamp(axis, -1f, 1f);
    }

    // Booléen pour le bouton saut
    public static bool GetButtonJump()
    {
        return Input.GetKey(KeyMappingSettings.Jump);
    }

    // Booléen pour le bouton tirer
    public static bool GetButtonShoot()
    {
        return Input.GetKey(KeyMappingSettings.Shoot);
    }
}
