using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ArmorPickup : MonoBehaviour
{
    public int addArmor = 2; 

    private void OnTriggerEnter(Collider other)
    {
        FPSInput fps = other.GetComponent<FPSInput>();
        if (fps != null && fps.shield != fps.maxShield)
        {
            fps.shield += addArmor;
            fps.playerShieldBar.enabled = true;
            Destroy(gameObject); // Destroy the health pickup object
        }
    }
}
