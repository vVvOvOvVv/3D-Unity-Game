using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRecovery : MonoBehaviour
{
    public int increaseHP = 2; 

    private void OnTriggerEnter(Collider other)
    {
        FPSInput fps = other.GetComponent<FPSInput>();
        if (fps != null && fps.currentHealth != fps.maxHealth) // no need to heal if max HP
        {
            fps.currentHealth = Mathf.Min(fps.currentHealth + increaseHP, fps.maxHealth);
            Destroy(gameObject); // Destroy the health pickup object
        }
    }
}
