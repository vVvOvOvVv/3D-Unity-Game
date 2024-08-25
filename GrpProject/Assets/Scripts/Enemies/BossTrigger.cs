using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BossTrigger : MonoBehaviour
{
    [SerializeField] private BossBehavior bossBehaviorScript;
    [SerializeField] private Boss bossScript;
    [SerializeField] private GameObject hitboxes; 
    private bool triggered; // ensure this is only triggered once

    private void Awake()
    {
        hitboxes.SetActive(false); // prevent player from being able to attack the boss before fight starts
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
        yield return new WaitForSeconds(1.21f);
        bossBehaviorScript.roar.Play(); 
        yield return new WaitForSeconds(4.03f);

        bossScript.hpBar.gameObject.SetActive(true);
        bossBehaviorScript.playerInRoom = true;
        hitboxes.SetActive(true);
        gameObject.SetActive(false);
    }
}
