using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* Placeholder for future extension of machinegun types idk how to implement those yet
 * public enum MachineGunType
{
    None,
    ShockMachineGun,
    PoisonMachineGun
}*/
// this is attached to the MachineGun prefab
public class MachineGun : Weapon
{
    public float fireRate = 0.1f; // time between each shot
    public float impulseStrength = 10f; // force applied to the bullet
    private bool isFiring = false;

    protected override void Start()
    {
        base.Start();
    } 
}
