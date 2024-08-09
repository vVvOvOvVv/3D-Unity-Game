using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : MonoBehaviour
{
    public float speedUp = 3.0f;
    public float dashPowerUp = 1.0f;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }
    private void OnTriggerEnter(Collider other)
    {
        FPSInput fps = player.GetComponent<FPSInput>();
        fps.speed += speedUp;
        fps.dashSpeed += dashPowerUp;
        Destroy(this.gameObject);
    }
}
