public class Grunt : Enemy
{
    private new void Awake()
    {
        maxHP = 10;
        hp = maxHP;
        dmgPerHit = 2;
        spawnChance = 100f; // starting spawn rate
        base.Awake();
    } 
}
