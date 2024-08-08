using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    private Camera cam;
    private Weapon currentWeapon;
    private bool showAim;
    private float nextFireTime = 0.3f;

    void Start()
    {
        cam = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnGUI()
    {
        if (showAim)
        {
            int size = 12;
            float posX = cam.pixelWidth / 2 - size / 4;
            float posY = cam.pixelHeight / 2 - size / 2;
            GUI.Label(new Rect(posX, posY, size, size), "*");
        }
    }

    void Update()
    {
        currentWeapon = Inventory.Instance.GetCurrentWeapon();
        if (currentWeapon != null)
        {
            if (Input.GetMouseButton(0)) // left mouse button held down
            {
                if (currentWeapon is MachineGun machineGun)
                {
                    if (Time.time >= nextFireTime)
                    {
                        machineGun.Shoot();
                        nextFireTime = Time.time + machineGun.fireRate;
                    }
                }
                else
                {
                    if (currentWeapon.canFire)
                    {
                        currentWeapon.Shoot();
                        currentWeapon.canFire = false; // prevent continuous firing for handguns/or other guns
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0)) // left mouse button released
            {
                if (currentWeapon is MachineGun mg)
                {
                    mg.StopFiring();
                }
                else
                {
                    currentWeapon.canFire = true; // allow firing again for handguns/or other guns
                }
            }

            if (Input.GetMouseButtonDown(1)) // right mouse button to aim
            {
                showAim = !showAim; // Toggle GUI visibility
            }
            if (Input.GetKeyDown(KeyCode.R)) // R for reload
            {
                currentWeapon.Reload();
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
    }
}
