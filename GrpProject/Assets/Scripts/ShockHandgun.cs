using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// this is attached to the ShockHandgun prefab (DOES NOT WORK PROPERLY)
public class ShockHandgun : Weapon
{
    public float impulseStrength = 5.0f;
    public float shockChance = 1.0f; // 100% chance to apply shock debuff
    public int shockDuration = 3; // Duration of shock effect in seconds
    public int damage = 2; // 

    public override void Shoot()
    {
        if (isReloading || currentAmmo <= 0 || !canFire)
            return;

        Vector3 point = new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2, 0);
        Ray ray = cam.ScreenPointToRay(point);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Hit: " + hit.collider.name);
            GameObject hitObject = hit.transform.gameObject;
            BruteBehavior brute = hitObject.GetComponent<BruteBehavior>();
            Shootable target = hitObject.GetComponent<Shootable>(); // check if the hit object is Shootable

            // if the object is shootable, apply damage
            if (target != null)
            {
                target.TakeDamage(damage); 
            }

            // If the object has BruteBehavior, apply the shock debuff
            if (brute != null)
            {
                Debug.Log("BruteBehavior component found on: " + brute.gameObject.name);
                if (Random.value < shockChance)
                {
                    ApplyShockDebuff(brute);
                }
            }
            else
            {
                Debug.Log("BruteBehavior component not found on: " + hitObject.name);
            }

            StartCoroutine(GeneratePS(hit));
        }

        currentAmmo--;
        canFire = false; // prevent continuous firing
    }

    private void ApplyShockDebuff(BruteBehavior brute)
    {
        Debug.Log("Applying shock debuff to: " + brute.gameObject.name);
        StartCoroutine(brute.ShockEnemy(shockDuration));
    }

    private IEnumerator GeneratePS(RaycastHit hit)
    {
        GameObject ps = Instantiate(particleSysPrefab, hit.point, Quaternion.LookRotation(hit.normal));
        yield return new WaitForSeconds(1);
        Destroy(ps);
    }
}
