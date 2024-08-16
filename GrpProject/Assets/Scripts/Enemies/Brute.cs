public class Brute : Enemy
{   
    private new void Awake()
    {
        maxHP = 30;
        hp = maxHP;
        dmgPerHit = 15;
        spawnChance = 0f; // starting spawn rate
        base.Awake();
    }
}
