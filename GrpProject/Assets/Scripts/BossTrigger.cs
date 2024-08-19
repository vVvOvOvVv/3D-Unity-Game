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
        bossBehaviorScript.enemyAnim.SetTrigger("Roar");

        yield return new WaitForSeconds(5.24f);

        bossBehaviorScript.playerInRoom = true;

        gameObject.SetActive(false);
    }
}
