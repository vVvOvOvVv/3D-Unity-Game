using System.Collections; 
using UnityEngine; 

public class Enemy : MonoBehaviour
{
    public GameObject smokePrefab; // hide enemy disappearing on death
    public GameObject weaponPickupPrefab; // can use the same prefab for the pickup
    public EnemyHPBar hpBar;
    public int hp, // health
        maxHP, // max health
        dmgPerHit,// enemy's dmg per hit/shot
        wpnDropRate; // weapon drops 1 out of X times
    public bool isDead;

    public void Awake()
    {
        wpnDropRate = 4; // standard drop rate
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
        Debug.Log("Burn effect on: " + gameObject.name);

        for (int i = 0; i < duration; i++)
        {
            TakeDamage(fireDmg);
            yield return new WaitForSeconds(1);
        }

        Debug.Log("Burn effect ended on: " + gameObject.name);
    }

    public IEnumerator HPDepleted()
    {
        isDead = true;
        // chance of weapon drop 
        /*  int wpnDropChance = Random.Range(1, wpnDropRate + 1);
          if (wpnDropChance == 1) */
        Instantiate(weaponPickupPrefab, transform.position, Quaternion.identity);

        // drop ammo - random amount within a range
        // to randomize which ammo type, just set random range as 0 or 1 (Range(0, 2)) to determine which 
        // currently equipped gun to replenish

        // create smoke effect to hide enemy disappearing
        GameObject smoke = Instantiate(smokePrefab, transform);
        yield return new WaitForSeconds(0.5f);
        Destroy(smoke);
        Destroy(gameObject);
    }
}
