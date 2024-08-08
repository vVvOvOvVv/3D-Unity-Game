using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    public Transform weaponHolder;
    public GameObject pistolPrefab;
    private Weapon currentWeapon;
    private GameObject lastPickedWeapon; // The last picked-up weapon
    private GameObject secondLastPickedWeapon;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Start with the pistol
        SwitchWeapon(pistolPrefab);
        lastPickedWeapon = pistolPrefab; // Initialize with pistol
        secondLastPickedWeapon = null;
    }

    public void SwitchWeapon(GameObject weaponPrefab)
    { 
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);
        }

        if (weaponPrefab != null)
        {
            GameObject weaponInstance = Instantiate(weaponPrefab, weaponHolder.position, weaponHolder.rotation);

            // Parent weapon to the weapon holder
            weaponInstance.transform.SetParent(weaponHolder);
            weaponInstance.transform.localPosition = Vector3.zero;  // Adjust as needed
            weaponInstance.transform.localRotation = Quaternion.identity;
            currentWeapon = weaponInstance.GetComponent<Weapon>();
        }
        else
        {
            Debug.LogError("Weapon prefab is null.");
        }
    }

    public Weapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public void AddWeapon(GameObject weaponPrefab)
    {
        // Update the weapon history
        if (lastPickedWeapon != null)
        {
            secondLastPickedWeapon = lastPickedWeapon;
        }

        lastPickedWeapon = weaponPrefab;

        // Always switch to the last picked weapon
        SwitchWeapon(lastPickedWeapon);
    }

    public void SwitchWeaponByIndex(int index)
    {
        if (index == 0 && secondLastPickedWeapon != null)
        {
            SwitchWeapon(secondLastPickedWeapon);
        }
        else if (index == 1 && lastPickedWeapon != null)
        {
            SwitchWeapon(lastPickedWeapon);
        }
        else
        {
            Debug.LogError("Invalid weapon index.");
        }
    }

    public void SwitchToNextWeapon()
    {
        // This method can be used if needed to cycle between weapons
    }
}
