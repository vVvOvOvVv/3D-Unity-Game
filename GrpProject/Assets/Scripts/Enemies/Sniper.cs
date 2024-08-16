public class Sniper : Enemy
{
    private new void Awake()
    {
        maxHP = 20;
        hp = maxHP;
        dmgPerHit = 25;
        spawnChance = 0; // starting spawn rate
        base.Awake();
    }
}
