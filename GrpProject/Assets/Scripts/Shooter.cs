using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    private Camera cam;

    private Weapon currentWeapon;

    private bool showAim;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnGUI()
    {
        if(showAim)
        {
            int size = 12;
            float posX = cam.pixelWidth / 2 - size / 4;
            float posY = cam.pixelHeight / 2 - size / 2;
            GUI.Label(new Rect(posX, posY, size, size), "*");
        }  
    }

    // Update is called once per frame
    void Update()
    {
        currentWeapon = Inventory.Instance.GetCurrentWeapon();
        if (currentWeapon != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                currentWeapon.Shoot();
            }
            if (Input.GetMouseButtonDown(1)) // Right mouse button to aim
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
