using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handgun : Weapon
{
    public float impulseStrength = 5.0f;

    public override void Shoot()
    {
        if (isReloading)
        {
            Debug.Log("Cannot shoot while reloading.");
            return;
        }

        if (currentAmmo <= 0)
        {
            Debug.Log("Out of ammo. Please reload.");
            return;
        }

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
            }

            StartCoroutine(GeneratePS(hit));
        }

        currentAmmo--;
    }

    private IEnumerator GeneratePS(RaycastHit hit)
    {
        GameObject ps = Instantiate(particleSysPrefab, hit.point, Quaternion.LookRotation(hit.normal));
        yield return new WaitForSeconds(1);
        Destroy(ps);
    }
}
