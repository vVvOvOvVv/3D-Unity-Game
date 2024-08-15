using System.Collections;
using UnityEngine;

public class WeaponPickupMenu : MonoBehaviour
{
    [SerializeField] private GameObject wpnPickupPrefab;
    private WeaponPickUp wpnPickupScript;
    [SerializeField] private Shooter shooterScript;
    [SerializeField] private GameObject inventoryPanel;

    private void Start()
    {
        wpnPickupScript = wpnPickupPrefab.GetComponent<WeaponPickUp>();
        inventoryPanel = GameObject.FindWithTag("InventoryPanel");
    }

    public void GetRandomWeapon()
    {
        wpnPickupScript.RandomWeapon();
        StartCoroutine(ShowInventory());
        CloseMenu();
    }

    public void UpgradeRandomWeapon()
    {
        Inventory.Instance.UpgradeRandomWeapon();
        StartCoroutine(ShowInventory());
        CloseMenu();
    } 

    public void CloseMenu()
    {
        // Unlock the cursor and make it visible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1.0f; // resume game
        shooterScript.gamePaused = false;
        gameObject.SetActive(false);
    }
     
    public IEnumerator ShowInventory()
    {
        inventoryPanel.SetActive(true);
        yield return new WaitForSeconds(1);
        // ideally create a fade "animation" for this panel
        inventoryPanel.SetActive(false);
    }
}
