using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : MonoBehaviour
{
    public float speedUp = 3.0f;
    public float dashPowerUp = 1.0f;
    public int timer = 5;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FPSInput fps = other.GetComponent<FPSInput>();
            fps.TempSpdUp(speedUp, dashPowerUp, timer);
            Destroy(gameObject); 
        }
    }
}
