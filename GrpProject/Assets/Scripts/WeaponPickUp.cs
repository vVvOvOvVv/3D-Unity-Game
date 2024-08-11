using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    //public GameObject weaponPrefab;
    //public float respawnTime = 5f;
    public GameObject[] weaponPrefabs; // equipped weapon prefabs 
    [SerializeField] private GameObject inventoryPanel;

    private Renderer render;
    private Collider collide;
    //private bool isPickedUp = false;

    void Start()
    {
        render = GetComponent<Renderer>();
        collide = GetComponent<Collider>();
        inventoryPanel = GameObject.FindWithTag("InventoryPanel");
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

        StartCoroutine(ShowInventory());

        // Add the weapon to the inventory
        Inventory.Instance.AddWeapon(randomWeapon);

        // Optionally hide the weapon pickup
        render.enabled = false;
        collide.enabled = false;

        // Optionally destroy the weapon pickup object
        Destroy(gameObject);
    }

    private IEnumerator ShowInventory()
    {
        inventoryPanel.SetActive(true);
        yield return new WaitForSeconds(1);
        // ideally create a fade "animation" for this panel
        inventoryPanel.SetActive(false);
    }
}
