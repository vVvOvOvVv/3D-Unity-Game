using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BossTrigger : MonoBehaviour
{
    [SerializeField] private BossBehavior bossBehaviorScript;
    private bool triggered; // ensure this is only triggered once

    private void Awake()
    {
        triggered = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            triggered = true;
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
