using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    //public GameObject weaponPrefab;
    //public float respawnTime = 5f;
    public GameObject[] weaponPrefabs;

    private Renderer renderer;
    private Collider collider;
    //private bool isPickedUp = false;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        collider = GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PickUp();
        }
    }

    /*void PickUp()
    {
        if (!isPickedUp)
        {
            Debug.Log("Picking up weapon: " + weaponPrefab.name);
            isPickedUp = true;
            renderer.enabled = false;
            collider.enabled = false;

            Inventory.Instance.SwitchWeapon(weaponPrefab);

            Invoke(nameof(Respawn), respawnTime);
        }
    }

    void Respawn()
    {
        Debug.Log("Respawning weapon: " + weaponPrefab.name);
        isPickedUp = false;
        renderer.enabled = true;
        collider.enabled = true;
    }*/

    void PickUp()
    {
        if (weaponPrefabs.Length == 0)
        {
            Debug.LogError("No weapon prefabs assigned.");
            return;
        }

        // Choose a random weapon from the array
        GameObject randomWeapon = weaponPrefabs[Random.Range(0, weaponPrefabs.Length)];

        Debug.Log("Picking up weapon: " + randomWeapon.name);

        // Add the weapon to the inventory
        Inventory.Instance.AddWeapon(randomWeapon);

        // Optionally hide the weapon pickup
        renderer.enabled = false;
        collider.enabled = false;

        // Optionally destroy the weapon pickup object
        Destroy(gameObject);
    }
}
