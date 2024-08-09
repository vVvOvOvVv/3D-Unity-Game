using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonHandgun : Weapon
{
    public float impulseStrength = 5.0f;
    public float poisonChance = 0.4f; // 40% chance to apply poison debuff
    public int poisonDuration = 5; // Duration of poison effect in seconds
    public float poisonSpeedFactor = 0.5f; // slow down to 50% speed 
}
