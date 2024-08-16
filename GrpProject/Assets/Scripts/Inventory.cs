using System.Collections;
using TMPro;
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
    [SerializeField] private TextMeshProUGUI gunLvlTxt1, gunLvlTxt2;

    // HUD
    [SerializeField]
    private GameObject inventoryPanel; // panel showing what guns are equipped
    // icons for the HUD
    [SerializeField]
    private GameObject[] gunIcons, // guns - idx 0-2 slot 1, idx 3-5 slot 2 
            // ensure order is handgun > shotgun > machinegun
        crosshairs, // ensure same order as gunIcons
        elementIcons; // element - no icon => normal/standard  - idx 0-2 slot 1, idx 3-5 slot 2 
            // ensure order is fire > shock > poison
    private static int IdxOffset = 3;

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
            ChangeCrosshair();
            // show inventory
            StartCoroutine(ShowInventory()); 
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

        // disable all weapon and element icons first
        for (int i = 0; i < gunIcons.Length; i++)
        {
            gunIcons[i].SetActive(false);
            elementIcons[i].SetActive(false);
        }
        // change inventory icons 
        UpdateInventoryHUD(secondLastPickedWeapon, 0);
        UpdateInventoryHUD(lastPickedWeapon, IdxOffset);

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

    private void UpdateInventoryHUD(GameObject weapon, int idxOffset)
    { 
        // reactivate relevent icons
        Weapon wpnScript = weapon.GetComponent<Weapon>();
        if (wpnScript != null)
        {
            // gun icons
            if (wpnScript.gunType == Weapon.GunType.Machinegun)
                gunIcons[2 + idxOffset].SetActive(true); 
            else if (wpnScript.gunType == Weapon.GunType.Handgun)  
                    gunIcons[0 + idxOffset].SetActive(true);
            else gunIcons[1 + idxOffset].SetActive(true); // shotgun (2 bullets per tap) 
            // element icons
            if (wpnScript.isFire)
                elementIcons[0 + idxOffset].SetActive(true);
            else if (wpnScript.isShock)
                elementIcons[1 + idxOffset].SetActive(true);
            else if (wpnScript.isPoison)
                elementIcons[2 + idxOffset].SetActive(true);
            // if all false, not need for element icons
        } else Debug.LogError("Weapon.cs not found on " + weapon.name);
    }

    private void ChangeCrosshair( )
    {
        // deactivate all crosshairs
        for (int i = 0; i < crosshairs.Length; i++)
            crosshairs[i].SetActive(false);
         
        if (currentWeapon != null)
        {
            if (currentWeapon.gunType == Weapon.GunType.Machinegun)
                crosshairs[2].SetActive(true);
            else if (currentWeapon.gunType == Weapon.GunType.Handgun)
                crosshairs[0].SetActive(true);
            else crosshairs[1].SetActive(true); // shotgun 
        } else crosshairs[0].SetActive(true); // pistol crosshair as default
    }

    public bool IsDuplicate(Weapon newWpn)
    {
        if (newWpn != null)
        {
            // at least 1 weapon is always equipped
            Weapon lastPicked = lastPickedWeapon.GetComponent<Weapon>();
            if (secondLastPickedWeapon != null) // additional weapon obtained
            {
                Weapon secondPicked = secondLastPickedWeapon.GetComponent<Weapon>();
                if ((newWpn.gunType.Equals(lastPicked.gunType) && newWpn.dmgType.Equals(lastPicked.dmgType)) ||
                    (newWpn.gunType.Equals(secondPicked.gunType) && newWpn.dmgType.Equals(secondPicked.dmgType)))
                    return true;
                else return false;
            }
            else // only 1 weapon in inventory
            {
                if (newWpn.gunType.Equals(lastPicked.gunType) && newWpn.dmgType.Equals(lastPicked.dmgType))
                    return true;
                else return false;
            }
        }
        else
        {
            Debug.LogError(newWpn.name + " not found");
            return false;
        }
    }

    public void UpgradeRandomWeapon()
    { 
        if (secondLastPickedWeapon == null) // no weapon picked up yet
            lastPickedWeapon.GetComponent<Weapon>().wpnLevel += 0.5f;
        else
        {
            int rand = Random.Range(0, 2);

            switch (rand)
            {
                case 0:
                    lastPickedWeapon.GetComponent<Weapon>().wpnLevel += 0.5f;
                    gunLvlTxt2.SetText("Lv. " + lastPickedWeapon.GetComponent<Weapon>().wpnLevel.ToString());
                    break;
                default: // 1
                    secondLastPickedWeapon.GetComponent<Weapon>().wpnLevel += 0.5f;
                    gunLvlTxt1.SetText("Lv. " + secondLastPickedWeapon.GetComponent<Weapon>().wpnLevel.ToString());
                    break;
            }
        }
    }
}
