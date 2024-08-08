using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour
{
    private bool armorActive = false; // Whether the player currently has armor

    public bool IsArmorActive()
    {
        return armorActive;
    }

    public void ActivateArmor(bool activate)
    {
        armorActive = activate;
        Debug.Log("Armor activated: " + armorActive);
    }
}
