using System.Collections; 
using UnityEngine; 

public class Enemy : MonoBehaviour
{
    [SerializeField] public GameObject smokePrefab, // hide enemy disappearing on death
        weaponPickupPrefab; // can use the same prefab for the pickup
    public int hp, // health
        dmgPerHit,// enemy's dmg per hit/shot
        wpnDropRate; // weapon drops 1 out of X times

    public Enemy()
    {
        hp = 0;
        dmgPerHit = 0;
        wpnDropRate = 4; // standard drop rate
    }

    public int GetHP() { return hp; }

    public int GetDmgPerHit() {  return dmgPerHit; }

    public void TakeDamage(int dmg)
    {
        hp -= dmg;

        if (hp <= 0) 
        { 
            StartCoroutine(HPDepleted());
        }
    }

    public IEnumerator HPDepleted()
    {
        // chance of weapon drop 
      /*  int wpnDropChance = Random.Range(1, wpnDropRate + 1);
        if (wpnDropChance == 1) */
        Instantiate(weaponPickupPrefab, transform); // randomization handled by WeaponPickup.cs

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
