using Unity.VisualScripting;
using UnityEngine;

public class Enemy
{
    [SerializeField] public GameObject enemyPrefab;
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
            // death animation or destroy object and instantiate a smoke particle
        }
    }
}
