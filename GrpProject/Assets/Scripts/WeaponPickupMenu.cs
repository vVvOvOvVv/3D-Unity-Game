using UnityEngine;

public class WeaponPickupMenu : MonoBehaviour
{
    [SerializeField] private GameObject wpnPickupPrefab;
    private WeaponPickUp wpnPickupScript;

    private void Start()
    {
        wpnPickupScript = wpnPickupPrefab.GetComponent<WeaponPickUp>();
    }

    public void GetRandomWeapon()
    {
        wpnPickupScript.RandomWeapon();
        CloseMenu();
    }

    public void UpgradeRandomWeapon()
    {
        Inventory.Instance.UpgradeRandomWeapon();
        CloseMenu();
    } 

    public void CloseMenu()
    {
        Time.timeScale = 1; // resume game 
        // Unlock the cursor and make it visible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        StartCoroutine(wpnPickupScript.ShowInventory());
    }
}
