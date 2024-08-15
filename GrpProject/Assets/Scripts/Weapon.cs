// for each weapon prefab, assign this script, and adjust values in Inspector
using System.Collections;
using UnityEngine; 

public class Weapon : MonoBehaviour
{

    // gun stats
    public int maxAmmo = 10; // Maximum ammo capacity    
    [SerializeField] public int currentAmmo;  // Current ammo count 
    public int dmg, spread, bulletsShot, bulletsPerTap, reserveAmmo;
    public float timeBetweenShots, reloadTime, timeBetweenShooting, impulseStrength; 
    private int normalSpread;

    // booleans - standard booleans
    public bool readyToShoot, isReloading, allowButtonHold, shooting;
    // booleans - determine if it deals special damage types
    public bool isFire, isPoison, isShock; // if all false => standard dmg type 

    // SHOCK LOGIC PROPERTIES
    private int shockChance = 5; // Initial chance (5%)
    private const int shockTime = 3; // Freeze time in seconds
    private const int maxShockChance = 25; // Max chance before reset
    private const int shockChanceIncrease = 2; // Chance increment per shot

    // POISON LOGIC PROPERTIES
    private const int poisonChance = 100; // 25% chance for poison effect
    [SerializeField] private static float PoisonSlowFactor = 0.7f; // slows enemy movement speed by 70%

    // constant variables
    [SerializeField] private static int 
        PoisonTime = 5,
        FireDmg = 1;
  

    // references
    protected Camera cam;
    public GameObject particleSysPrefab; 

    private void Start()
    {
        cam = Camera.main;
        currentAmmo = maxAmmo;
        readyToShoot = true;
        reserveAmmo = maxAmmo * 3;
        normalSpread = spread;
    }
 
    public void Shoot() 
    {
        readyToShoot = false;

        // if player is moving, increase spread
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            spread *= 2;
        else spread = normalSpread;
        // shot spread
        int x = Random.Range(-spread, spread);
        int y = Random.Range(-spread, spread);

        // get direction considering spread
        Vector3 direction = new Vector3((cam.pixelWidth / 2) + x, (cam.pixelHeight / 2) + y, 0); 
        Ray ray = cam.ScreenPointToRay(direction);
        RaycastHit hit;

        // raycast shot
        if (Physics.Raycast(ray, out hit)) // i wanted to implement range but alas, no dice
        {
            // get the GameObject that was hit
            GameObject hitObject = hit.transform.gameObject;
            Debug.Log("Hit: " + hitObject.name); // debug log

            Rigidbody rBody = hitObject.GetComponent<Rigidbody>();
            if (rBody != null)
            {
                Vector3 impulse = Vector3.Normalize(hit.point - cam.transform.position) * impulseStrength;
                hit.rigidbody.AddForceAtPosition(impulse, hit.point, ForceMode.Impulse); 
            }
             
            Enemy enemy = hitObject.GetComponent<Enemy>();
            if (enemy != null) 
            {
                enemy.TakeDamage(dmg);

                // dmg types
                // fire
                if (isFire)
                {

                }

                // SHOCK LOGIC
                if (isShock)
                {
                    ApplyShockEffect(hitObject);
                }
                // POISON LOGIC
                if (isPoison)
                {
                    // hitObject.GetComponent<Behavior>().PoisonEnemy(PoisonTime, PoisonSlowFactor);
                    ApplyPoisonEffect(hitObject);
                }
            }

            StartCoroutine(GeneratePS(hit));
        }
        else Debug.Log("No hit :(");

        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 1f); // Visualize the ray
 

        currentAmmo--;
        bulletsShot--;
        // only allow player to fire again after set time - rate of fire
        Invoke("ResetShot", timeBetweenShooting);
        // in the case of multiple shots firing at once
        if (bulletsShot > 0 && currentAmmo > 0)
            Invoke("Shoot", timeBetweenShots); 
    }

    // SHOCK LOGIC
    private void ApplyShockEffect(GameObject hitObject)
    {
        shockChance = 100; // temporary 100% chance for debugging purp
        int rand = Random.Range(0, 100); // random number between 0 and 99

        if (rand < shockChance)
        {
            // Shock effect is applied
            Behavior behaviorScript = hitObject.GetComponent<Behavior>();
            if (behaviorScript != null)
            {
                StartCoroutine(behaviorScript.ShockEnemy(shockTime));
            }
            shockChance = 5; // reset shock chance after successful shock
        }
        else
        {
            // increase shock chance for next shot
            shockChance += shockChanceIncrease;
            shockChance = Mathf.Min(shockChance, maxShockChance); // cap the chance at a maximum value
        }
    }

    // POISON LOGIC
    private void ApplyPoisonEffect(GameObject hitObject)
    {
        int rand = Random.Range(0, 100); // Generate a random number between 0 and 99

        if (rand < poisonChance)
        {
            // Poison effect is applied
            Behavior behaviorScript = hitObject.GetComponent<Behavior>();
            if (behaviorScript != null)
            {
                StartCoroutine(behaviorScript.PoisonEnemy(PoisonTime, PoisonSlowFactor));
            }
        }
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
        if (reserveAmmo < (maxAmmo - currentAmmo)) // reserve ammo less than ammo needed to refill to max
        {
            currentAmmo += reserveAmmo;
            reserveAmmo = 0;
        } else
        {
            reserveAmmo -= maxAmmo - currentAmmo;
            currentAmmo = maxAmmo;
        }
        isReloading = false;
    }

    public void AddAmmo(int ammoAdded)
    {
        // upon pick up, add additional ammo to reserve
        reserveAmmo += ammoAdded;
    }
 
    private IEnumerator GeneratePS(RaycastHit hit) 
    { 
        GameObject ps = Instantiate(particleSysPrefab, hit.point, Quaternion.LookRotation(hit.normal));
        yield return new WaitForSeconds(1);
        Destroy(ps);
    }
}
