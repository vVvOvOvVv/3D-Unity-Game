using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonHandgun : Weapon
{
    public float impulseStrength = 5.0f;
    public float poisonChance = 0.4f; // 40% chance to apply poison debuff
    public int poisonDuration = 5; // Duration of poison effect in seconds
    public float poisonSpeedFactor = 0.5f; // slow down to 50% speed

    public override void Shoot()
    {
        if (isReloading || currentAmmo <= 0 || !canFire)
            return;

        Vector3 point = new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2, 0);
        Ray ray = cam.ScreenPointToRay(point);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.transform.gameObject;
            Shootable target = hitObject.GetComponent<Shootable>();
            if (target != null)
            {
                Vector3 impulse = Vector3.Normalize(hit.point - cam.transform.position) * impulseStrength;
                hit.rigidbody.AddForceAtPosition(impulse, hit.point, ForceMode.Impulse);
                target.TakeDamage(1);

                // Check if the hit object has BruteBehavior and apply poison debuff
                BruteBehavior brute = target.GetComponent<BruteBehavior>();
                if (brute != null)
                {
                    ApplyPoisonDebuff(brute);
                }
            }

            StartCoroutine(GeneratePS(hit));
        }
        currentAmmo--;
        canFire = false; // prevent continuous firing
    }

    private void ApplyPoisonDebuff(BruteBehavior brute)
    {
        Debug.Log("Applying poison debuff to: " + brute.gameObject.name);
        StartCoroutine(brute.PoisonEnemy(poisonDuration, poisonSpeedFactor));
    }

    private IEnumerator GeneratePS(RaycastHit hit)
    {
        GameObject ps = Instantiate(particleSysPrefab, hit.point, Quaternion.LookRotation(hit.normal));
        yield return new WaitForSeconds(1);
        Destroy(ps);
    }
}
