using UnityEngine;

public class WithinBossRange : MonoBehaviour
{
    [SerializeField] private BossBehavior bossBehaviorScript;
    [SerializeField] public bool isSwipe, isJump; // to reuse the script for different attacks

    private void Awake()
    {
        if (bossBehaviorScript == null)
            bossBehaviorScript = GetComponentInParent<BossBehavior>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isSwipe)
                StartCoroutine(bossBehaviorScript.SwipeAttack());
            else if (isJump)
                StartCoroutine(bossBehaviorScript.JumpAttack());
        }
    }
}
