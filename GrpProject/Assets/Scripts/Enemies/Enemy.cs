using System.Collections; 
using UnityEngine; 

public class Enemy : MonoBehaviour
{
    [SerializeField] public GameObject smokePrefab;
    public int hp,
        dmgPerHit; 

    public Enemy()
    {
        hp = 0;
        dmgPerHit = 0;
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
        // create smoke effect to hide enemy disappearing
        GameObject smoke = Instantiate(smokePrefab, transform);
        yield return new WaitForSeconds(0.5f);
        Destroy(smoke);
        Destroy(gameObject);
    }
}
