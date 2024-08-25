using UnityEngine;

[System.Serializable]
public class Attack // to control the attack damage per attack
{
    [SerializeField] public string name; // identification in inspector
    [SerializeField] public int dmg;
}

public class BossAttack : MonoBehaviour
{
    [SerializeField] private Attack atk;
    [SerializeField] private BossBehavior behaviorScript;
    [SerializeField] private Boss bossScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && behaviorScript.isAttacking && !bossScript.isDead) // look for player
        {
            FPSInput fps = other.GetComponent<FPSInput>();
            fps.TakeDamage(atk.dmg);
        }
    }
}
