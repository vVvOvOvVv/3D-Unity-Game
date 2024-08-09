using UnityEngine; 

public class Weapon : MonoBehaviour
{

    // gun stats
    public int maxAmmo = 10; // Maximum ammo capacity    
    [SerializeField] public int currentAmmo;  // Current ammo count
    public int dmg;
    public float timeBetweenShots, normalSpread, spread, range, reloadTime = 1.5f, timeBetweenShooting;
    public int bulletsShot, bulletsPerTap, reserveAmmo;

    // booleans
    public bool readyToShoot, isReloading;

    // references
    protected Camera cam;
    public GameObject particleSysPrefab;
    public RaycastHit hit;
    public LayerMask whatIsEnemy;

    protected virtual void Start()
    {
        cam = Camera.main;
        currentAmmo = maxAmmo;
        readyToShoot = true;
    }

    public virtual void Shoot()
    {
        readyToShoot = false;

        // if player is moving, increase spread
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            spread *= 1.5f;
        else spread = normalSpread;
        // shot spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        // get direction considering spread
        Vector3 direction = cam.transform.forward + new Vector3(x, y, 0);

        // raycast shot
        if (Physics.Raycast(cam.transform.position, direction, out hit, range, whatIsEnemy))
        {
            Debug.Log("Hit: " + hit.collider.name); // debug log

            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
                enemy.TakeDamage(dmg);
        }

        currentAmmo--;
        bulletsShot--;
        // only allow player to fire again after set time - rate of fire
        Invoke("ResetShot", timeBetweenShooting);
        // in the case of multiple shots firing at once
        if (bulletsShot > 0 && currentAmmo > 0)
            Invoke("Shoot", timeBetweenShots);
    }

    public void ResetShot()
    {
        readyToShoot = true;
    }

    public void Reload()
    {
        isReloading = true;
        Invoke("ReloadFinished", reloadTime); 
    }

    public void ReloadFinished()
    {
        currentAmmo = maxAmmo;
        isReloading = false;
    }

    public void AddAmmo(int ammoAdded)
    {
        // upon pick up, add additional ammo to reserve
        reserveAmmo += ammoAdded;
    }
}
