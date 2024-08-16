using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRecovery : MonoBehaviour
{
    public int increaseHP = 10; 

    private void OnTriggerEnter(Collider other)
    { 
        if (other.CompareTag("Player"))
        {
            FPSInput fps = other.GetComponent<FPSInput>();
            if (fps.currentHealth < fps.maxHealth) // only need to restore HP if < maxHP
            {
                if (fps.maxHealth - fps.currentHealth < increaseHP) // cannot exceed max HP
                    increaseHP += fps.currentHealth - fps.maxHealth;
                fps.Heal(increaseHP);
                Destroy(gameObject); // Destroy the health pickup object 
            }
        }
    }
}
