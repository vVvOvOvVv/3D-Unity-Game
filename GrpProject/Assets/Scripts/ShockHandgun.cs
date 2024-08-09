using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// this is attached to the ShockHandgun prefab (DOES NOT WORK PROPERLY)
public class ShockHandgun : Weapon
{
    public float impulseStrength = 5.0f;
    public float shockChance = 1.0f; // 100% chance to apply shock debuff
    public int shockDuration = 3; // Duration of shock effect in seconds
    public int damage = 2; //  

    private void ApplyShockDebuff(BruteBehavior brute)
    {
        Debug.Log("Applying shock debuff to: " + brute.gameObject.name);
        StartCoroutine(brute.ShockEnemy(shockDuration));
    } 
}
