using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected Camera cam;
    public GameObject particleSysPrefab;

    public int maxAmmo = 10; // Maximum ammo capacity    
    [SerializeField] public int currentAmmo;  // Current ammo count

    protected bool isReloading = false;
    public float reloadTime = 1.5f;
    public bool canFire = true; 

    protected virtual void Start()
    {
        cam = Camera.main;
        currentAmmo = maxAmmo;
    }

    public abstract void Shoot();

    public virtual void Reload()
    {
        if (isReloading || currentAmmo == maxAmmo) // No need to reload if already full
            return;

        StartCoroutine(ReloadCoroutine());
    }

    private IEnumerator ReloadCoroutine()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        // Play reload animation or sound if you have any
        // e.g., reloadAnimation.Play();

        yield return new WaitForSeconds(reloadTime);

        // Finish reloading
        currentAmmo = maxAmmo; // Refill ammo
        isReloading = false;
        Debug.Log("Reloading complete.");
    }
}
