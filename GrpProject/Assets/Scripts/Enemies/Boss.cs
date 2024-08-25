using System.Collections;
using UnityEngine;

[RequireComponent (typeof(BossBehavior))]
public class Boss : Enemy
{
    private BossBehavior behaviorScript;
    [SerializeField] private GameObject victoryCanvas;
    [SerializeField] private Shooter shooterScript;

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
        // Debug.Log($"Critical Hit! Damage: {critDamage}, Boss HP Remaining: {hp}");

        if (hpBar != null)
            hpBar.UpdateHPBar(hp, maxHP);
        else
            Debug.LogError("HP DONDE ESTA");

        if (hp <= 0 && !isDead) // prevent repeat death 
            StartCoroutine(BossHPDepleted());
    }

    public new void TakeDamage(int dmg)
    {
        hp -= dmg;
         Debug.Log($"Normal Hit! Damage: {dmg}, Boss HP Remaining: {hp}");

        if (hpBar != null)
            hpBar.UpdateHPBar(hp, maxHP);
        else Debug.LogError("HP Bar missing from boss!");

        if (hp <= 0 && !isDead) // prevent repeat death 
            StartCoroutine(BossHPDepleted()); 
    }

    public IEnumerator BossHPDepleted()
    {
        isDead = true;
         
        behaviorScript.agent.isStopped = true;
        // call on behavior script's death animation
        behaviorScript.enemyAnim.SetTrigger("Death");

        // wait for animation to finish
        yield return new WaitForSeconds(4.4f);

        // display victory screen
        victoryCanvas.SetActive(true);
        shooterScript.gamePaused = true;
        //unlock and display cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
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
