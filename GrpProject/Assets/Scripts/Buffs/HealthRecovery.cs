using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRecovery : MonoBehaviour
{
    public int increaseHP = 2;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        FPSInput fps = player.GetComponent<FPSInput>();
        if (fps != null)
        {
            fps.currentHealth = Mathf.Min(fps.currentHealth + increaseHP, fps.maxHealth);
            Destroy(gameObject); // Destroy the health pickup object
        }
    }
}
