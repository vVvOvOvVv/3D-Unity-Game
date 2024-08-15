using UnityEngine;

public class WeaponPickupMenu : MonoBehaviour
{
    [SerializeField] private GameObject wpnPickupPrefab;
    private WeaponPickUp wpnPickupScript;
    [SerializeField] private Shooter shooterScript;

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
        // Unlock the cursor and make it visible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1.0f; // resume game
        shooterScript.gamePaused = false;
        gameObject.SetActive(false);
    }
}
