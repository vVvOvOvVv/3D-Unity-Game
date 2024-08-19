using UnityEngine;

public class WithinBossRange : MonoBehaviour
{
    [SerializeField] private BossBehavior bossBehaviorScript;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            StartCoroutine(bossBehaviorScript.SwipeAttack());
    }
}
