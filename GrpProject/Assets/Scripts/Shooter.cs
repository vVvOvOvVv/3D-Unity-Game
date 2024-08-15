using UnityEngine;
using TMPro;
using EZCameraShake;

public class Shooter : MonoBehaviour
{  
    [SerializeField] private Weapon currentWeapon;   

    // TMPro elements from the HUD
    [SerializeField] private TextMeshProUGUI currentAmmoTxt, reserveAmmoTxt;

    public bool gamePaused;

    // determine method of fire - hold down or click LMB
    private void ShootInput()
    {
        if (currentWeapon.allowButtonHold)
            currentWeapon.shooting = Input.GetKey(KeyCode.Mouse0);
        else currentWeapon.shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) &&
            currentWeapon.currentAmmo < currentWeapon.maxAmmo && !currentWeapon.isReloading)
            currentWeapon.Reload();

        // shoot
        if (currentWeapon.readyToShoot && currentWeapon.shooting && 
            !currentWeapon.isReloading && currentWeapon.currentAmmo > 0)
        {
            currentWeapon.bulletsShot = currentWeapon.bulletsPerTap;
            CameraShaker.Instance.ShakeOnce(1f, 1f, 0.1f, 0.1f);
            currentWeapon.Shoot();
        }

        // RMB to aim - hold gun closer to camera and lower weapon spread 
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            // animations required
            
            Debug.Log("Aiming..."); // debug for now
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
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;
        gamePaused = false;
    } 

    void Update()
    {
        currentWeapon = Inventory.Instance.GetCurrentWeapon();
        if (currentWeapon != null && !gamePaused)
        {
            ShootInput();  
            // set text - ammo count
            currentAmmoTxt.SetText(currentWeapon.currentAmmo.ToString());
            reserveAmmoTxt.SetText(currentWeapon.reserveAmmo.ToString());
        } 
    }
}
