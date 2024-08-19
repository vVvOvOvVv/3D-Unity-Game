using System.Collections; 
using UnityEngine; 

public class Enemy : MonoBehaviour
{
    public GameObject smokePrefab; // hide enemy disappearing on death
    public GameObject weaponPickupPrefab, hpRecoveryPrefab, armorPrefab, speedUpPrefab, // pick-up-ables
        fireParticlePrefab; // status ailment indicator
    public EnemyHPBar hpBar;
    public int hp, // health
        maxHP, // max health
        dmgPerHit,// enemy's dmg per hit/shot
        wpnDropRate, hpDropRate, armorDropRate, spdDropRate; // drops 1 out of X times
    public bool isDead; 

    public void Awake()
    {
        // standard drop rates
        wpnDropRate = 5; // 20% chance
        hpDropRate = 5;
        armorDropRate = 4; // 25% chance
        spdDropRate = 3; // 33.3% chance

        isDead = false;
        if (hpBar != null)
            hpBar.UpdateHPBar(hp, maxHP);
        else
            Debug.LogError("HP DONDE ESTA");
    }

    public int GetHP() { return hp; }

    public int GetDmgPerHit() {  return dmgPerHit; }

    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        if (hpBar != null)
            hpBar.UpdateHPBar(hp, maxHP);
        else Debug.LogError("HP DONDE ESTA");

        if (hp <= 0 && !isDead) // prevent repeat death
        { 
            StartCoroutine(HPDepleted());
        }
    }

    // FIRE DAMAGE OVER TIME
    public IEnumerator BurnEnemy(int fireDmg, int duration)
    {
        if (!isDead)
        {
            // Debug.Log("Burn effect on: " + gameObject.name);
            GameObject fire = Instantiate(fireParticlePrefab, transform);

            for (int i = 0; i < duration; i++)
            {
                TakeDamage(fireDmg);
                yield return new WaitForSeconds(1);
            }

            Destroy(fire);
            // Debug.Log("Burn effect ended on: " + gameObject.name);
        }
        
    }

    public IEnumerator HPDepleted()
    {
        isDead = true;

        // randomize pick up drops
        // chance of weapon drop 
        int pickupDropChance = Random.Range(0, wpnDropRate);
        if (pickupDropChance == 0)
            Instantiate(weaponPickupPrefab, transform.position, Quaternion.identity);
        // chance of HP recovery drop
        pickupDropChance = Random.Range(0, hpDropRate);
        if (pickupDropChance == 0)
            Instantiate(hpRecoveryPrefab, transform.position, Quaternion.identity);
        // chance of shield/armor drop
        pickupDropChance = Random.Range(0, armorDropRate);
        if (pickupDropChance == 0)
            Instantiate(armorPrefab, transform.position, Quaternion.identity);
        // chance of speed up drop
        pickupDropChance = Random.Range(0, spdDropRate);
        if (pickupDropChance == 0)
            Instantiate(speedUpPrefab, transform.position, Quaternion.identity); 

        // create smoke effect to hide enemy disappearing
        GameObject smoke = Instantiate(smokePrefab, transform);
        yield return new WaitForSeconds(0.5f);
        Destroy(smoke);
        Destroy(gameObject);
    } 
}
