using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* Placeholder for future extension of machinegun types idk how to implement those yet
 * public enum MachineGunType
{
    None,
    ShockMachineGun,
    PoisonMachineGun
}*/
// this is attached to the MachineGun prefab
public class MachineGun : Weapon
{
    public float fireRate = 0.1f; // time between each shot
    public float impulseStrength = 10f; // force applied to the bullet
    private bool isFiring = false;

    protected override void Start()
    {
        base.Start();
    }

    public override void Shoot()
    {
        if (currentAmmo <= 0 || isReloading)
            return;

        StartCoroutine(FireContinuously());
    }

    private IEnumerator FireContinuously()
    {
        isFiring = true;

        while (isFiring && currentAmmo > 0)
        {
            FireBullet();
            currentAmmo--;

            // Instantiate particle system or other effects
            if (particleSysPrefab != null)
            {
                Instantiate(particleSysPrefab, cam.transform.position + cam.transform.forward * 2, Quaternion.LookRotation(cam.transform.forward));
            }

            // Wait for next shot
            yield return new WaitForSeconds(fireRate);
        }

        isFiring = false;
    }

    private void FireBullet()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // hit logic
            Debug.Log("Hit: " + hit.collider.name);

            Shootable target = hit.transform.GetComponent<Shootable>();
            if (target != null)
            {
                target.TakeDamage(1); // adjustable damage value
                Vector3 impulse = Vector3.Normalize(hit.point - cam.transform.position) * impulseStrength;
                hit.rigidbody.AddForceAtPosition(impulse, hit.point, ForceMode.Impulse);
            }

            StartCoroutine(GeneratePS(hit));
        }
    }

    private IEnumerator GeneratePS(RaycastHit hit)
    {
        GameObject ps = Instantiate(particleSysPrefab, hit.point, Quaternion.LookRotation(hit.normal));
        yield return new WaitForSeconds(1);
        Destroy(ps);
    }

    public void StopFiring()
    {
        isFiring = false;
    }

    public bool IsFiring()
    {
        return isFiring;
    }
}
