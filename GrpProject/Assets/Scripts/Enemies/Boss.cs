using UnityEngine;

[RequireComponent (typeof(BossBehavior))]
public class Boss : Enemy
{
    private BossBehavior behaviorScript;

    private new void Awake()
    {
        // no drop rate

        isDead = false;
        hp = maxHP;

        // ensure to assign HP Bar
        if (hpBar != null)
            hpBar.UpdateHPBar(hp, maxHP);
        else Debug.LogError("Boss's HP Bar missing!");
        behaviorScript = GetComponent<BossBehavior>();
    }

    /* public void CritHit(int dmg)
    {
        // take double damage on crit hit
        hp -= Mathf.FloorToInt(dmg * 1.5f);
        if (hpBar != null)
            hpBar.UpdateHPBar(hp, maxHP);
        else Debug.LogError("HP DONDE ESTA");

        if (hp <= 0 && !isDead) // prevent repeat death 
            BossHPDepleted(); 
    } */

    public new void TakeDamage(int dmg)
    {
        hp -= dmg;
        if (hpBar != null)
            hpBar.UpdateHPBar(hp, maxHP);
        else Debug.LogError("HP Bar missing from boss!");

        if (hp <= 0 && !isDead) // prevent repeat death 
            BossHPDepleted(); 
    }

    public void BossHPDepleted()
    {
        isDead = true;
        behaviorScript.canAttack = false;

        // call on behavior script's death animation
        behaviorScript.enemyAnim.SetTrigger("Death");
    }

    private void Update()
    {
        if (hp <= (maxHP / 4))
            behaviorScript.phase3 = true;
        else if (hp <= (maxHP / 2))
            behaviorScript.phase2 = true;
    }
}
