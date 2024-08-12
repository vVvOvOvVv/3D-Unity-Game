using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    public Transform weaponHolder;
    public GameObject pistolPrefab;
    private Weapon currentWeapon;
    private GameObject lastPickedWeapon; // The last picked-up weapon
    private GameObject secondLastPickedWeapon;

    // HUD
    [SerializeField]
    private GameObject pistolCrosshair, shotgunCrosshair, machinegunCrosshair, // crosshairs
        inventoryPanel; // panel showing what guns are equipped
    // icons for the HUD
    [SerializeField]
    private Sprite
        pistolIcon, shotgunIcon, machinegunIcon, // guns
        fireIcon, poisonIcon, shockIcon; // element - no icon => normal/standard
    [SerializeField] private Image[] gunIcons, elementIcons; // HUD

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
        inventoryPanel = GameObject.FindWithTag("InventoryPanel");

        // no starting elements
        elementIcons[0].GetComponent<Image>().enabled = false;
        elementIcons[1].GetComponent<Image>().enabled = false;
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

            // update HUD
            // change crossair
            // show inventory
            StartCoroutine(ShowInventory());

            // make inventory panel appear for a second and have it fade over time
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

        // change inventory icons
        gunIcons[1].sprite = gunIcons[0].sprite;
        if (currentWeapon.allowButtonHold) // machinegun
            gunIcons[0].sprite = machinegunIcon;
        else
        {
            if (currentWeapon.bulletsPerTap == 1) // pistol/handgun
                gunIcons[0].sprite = pistolIcon;
            else // shotgun (more than 1 bullet per tap)
                gunIcons[0].sprite = shotgunIcon;
        }

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

    private IEnumerator ShowInventory()
    {
        inventoryPanel.SetActive(true);
        yield return new WaitForSeconds(1);
        // ideally create a fade "animation" for this panel
        inventoryPanel.SetActive(false);
    }
}
