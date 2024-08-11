using System.Collections; 
using UnityEngine; 

public class Enemy : MonoBehaviour
{
    [SerializeField] public GameObject smokePrefab; // hide enemy disappearing on death
    [SerializeField] public GameObject weaponPickupPrefab; // can use the same prefab for the pickup
    [SerializeField] public EnemyHPBar hpBar;
    public int hp, // health
        maxHP, // max health
        dmgPerHit,// enemy's dmg per hit/shot
        wpnDropRate; // weapon drops 1 out of X times
    public bool isDead;

    public void Start()
    {
        hpBar = GetComponentInChildren<EnemyHPBar>();
        hpBar.UpdateHPBar(hp, maxHP);
    }

    public Enemy()
    {
        maxHP = 10;
        hp = maxHP; 
        dmgPerHit = 0;
        wpnDropRate = 4; // standard drop rate
        isDead = false;
    }

    public int GetHP() { return hp; }

    public int GetDmgPerHit() {  return dmgPerHit; }

    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        hpBar.UpdateHPBar(hp, maxHP);

        if (hp <= 0 && !isDead) // prevent repeat death
        { 
            StartCoroutine(HPDepleted());
        }
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
