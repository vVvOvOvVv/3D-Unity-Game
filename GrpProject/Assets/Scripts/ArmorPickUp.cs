using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorPickUp : MonoBehaviour
{
    public bool armorActive = true; // Whether this pickup provides armor
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }
    private void OnTriggerEnter(Collider other)
    {
        // Give armor to the player
        Armor armor = player.GetComponent<Armor>();
        if (armor != null)
        {
            armor.ActivateArmor(armorActive);
            Destroy(gameObject); // Destroy the armor pickup object
        }
    }
}
