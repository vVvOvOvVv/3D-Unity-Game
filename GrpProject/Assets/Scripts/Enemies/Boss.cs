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
         
        if (hpBar == null)
            hpBar = GameObject.FindWithTag("BossHPBar").GetComponent<EnemyHPBar>();
        if (hpBar != null)
        {
            hpBar.UpdateHPBar(hp, maxHP);
            hpBar.gameObject.SetActive(false); // hide from player until fight initiated
        }
        else Debug.LogError("Boss's HP Bar missing!");
        behaviorScript = GetComponent<BossBehavior>();
    }

    public void CritHit(int dmg)
    {
        int critDamage = Mathf.FloorToInt(dmg * 1.5f);
        hp -= critDamage;

        // Debug log to track critical hit damage
        Debug.Log($"Critical Hit! Damage: {critDamage}, Boss HP Remaining: {hp}");

        if (hpBar != null)
            hpBar.UpdateHPBar(hp, maxHP);
        else
            Debug.LogError("HP DONDE ESTA");

        if (hp <= 0 && !isDead) // prevent repeat death 
            BossHPDepleted();
    }

    public new void TakeDamage(int dmg)
    {
        hp -= dmg;
        Debug.Log($"Normal Hit! Damage: {dmg}, Boss HP Remaining: {hp}");

        if (hpBar != null)
            hpBar.UpdateHPBar(hp, maxHP);
        else Debug.LogError("HP Bar missing from boss!");

        if (hp <= 0 && !isDead) // prevent repeat death 
            BossHPDepleted(); 
    }

    public void BossHPDepleted()
    {
        isDead = true; 

        // call on behavior script's death animation
        behaviorScript.enemyAnim.SetTrigger("Death");

        // wait for animation to finish


        // display victory screen

    }

    private void Update()
    {
        if (hp <= (maxHP / 2))
        {
            behaviorScript.jumpColliders.SetActive(true);
            behaviorScript.phase2 = true;
        }
    }
}
