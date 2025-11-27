using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CryogenicBoxGroup", menuName = "Boxes/CryogenicBoxGroup")]
public class CryogenicBoxGroup : ScriptableObject
{
    public List<CryogenicBox> boxes;

    // Calcul des PV totaux non cassés dans le groupe
    public int CalculateTotalPV()
    {
        int total = 0;
        foreach (var box in boxes)
        {
            if (box != null && !box.IsBroken())
                total += box.GetCurrentPV();
        }
        return total;
    }

    // Nombre de caissons encore intacts
    public int CalculateRemainingBoxes()
    {
        int count = 0;
        foreach (var box in boxes)
        {
            if (box != null && !box.IsBroken())
                count++;
        }
        return count;
    }

    // Vérifie si tous les caissons sont détruits
    public bool isTotalDestruction()
    {
        return CalculateRemainingBoxes() == 0;
    }
}

// A METTRE DANS LE GAME MANAGER :
/*
public CryogenicBoxGroup groupBoxes; // à assigner dans l’inspecteur

void Update()
{
    if (groupBoxes.isTotalDestruction())
    {
        Debug.Log("Game Over : tous les caissons sont détruits !");
        // On déclenche la logique de fin de jeu
    }
}
*/