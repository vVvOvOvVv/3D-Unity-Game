using UnityEngine;
using TMPro;

public class Shooter : MonoBehaviour
{
    private Camera cam;
    private Weapon currentWeapon; 
    private float nextFireTime = 0.3f;

    // gun-related variables to determine method and ability to shoot
    public bool allowButtonHold, shooting;

    // TMPro elements from the HUD
    [SerializeField] private TextMeshProUGUI currentAmmoTxt, reserveAmmoTxt;

    // determine method of fire - hold down or click LMB
    private void ShootInput()
    {
        if (allowButtonHold)
            shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) &&
            currentWeapon.currentAmmo < currentWeapon.maxAmmo && currentWeapon.isReloading)
            currentWeapon.Reload();

        // shoot
        if (currentWeapon.readyToShoot && shooting && !currentWeapon.isReloading && currentWeapon.currentAmmo > 0)
        {
            currentWeapon.bulletsShot = currentWeapon.bulletsPerTap;
            currentWeapon.Shoot();
        }

        // RMB to aim - hold gun closer to camera and lower weapon spread 
        if (Input.GetMouseButtonDown(1))
        {
            // animations required
            // debug for now
            Debug.Log("Aiming...");
        }

        // Switch weapons based on key input
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Inventory.Instance.SwitchWeaponByIndex(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Inventory.Instance.SwitchWeaponByIndex(1);
        }
    }

    void Start()
    {
        cam = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    } 

    void Update()
    {
        currentWeapon = Inventory.Instance.GetCurrentWeapon();
        if (currentWeapon != null)
        {
            ShootInput(); 
        }

        // set text - ammo count
        currentAmmoTxt.SetText(currentWeapon.currentAmmo.ToString());
        reserveAmmoTxt.SetText(currentWeapon.reserveAmmo.ToString());
    }
}
