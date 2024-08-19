using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BossTrigger : MonoBehaviour
{
    [SerializeField] private BossBehavior bossBehaviorScript; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(RoarAnimation());
        }
    }

    private IEnumerator RoarAnimation()
    {
        bossBehaviorScript.state = BossBehavior.EnemyState.Roar;
        bossBehaviorScript.AnimationHandler();

        yield return new WaitForSeconds(5.24f);

        bossBehaviorScript.playerInRoom = true;
    }
}
