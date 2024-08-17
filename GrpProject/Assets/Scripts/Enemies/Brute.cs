public class Brute : Enemy
{   
    private new void Awake()
    {
        maxHP = 30;
        hp = maxHP;
        dmgPerHit = 15; 
        base.Awake();
    }
}
