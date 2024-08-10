using System.Collections;
using UnityEngine;
using UnityEngine.AI; // required for NavMesh

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent (typeof(CapsuleCollider))]
public class BruteBehavior : Behavior
{ 
    public override IEnumerator AgentNearPlayer()
    {
        // when the agent is about 10m away from the player, stop for a moment, mark player's position at this time
        // run at the mark, dealing damage to the player on collision
        float distanceToPlayer = Vector3.Distance(agent.transform.position, playerTransform.position);
        if (distanceToPlayer <= 15.0f)
        {
            agent.isStopped = true;
            Vector3 target = playerTransform.position;
            agent.destination = target;
            yield return new WaitForSeconds(1); // charge animation
            agent.speed *= 1.5f;
            agent.isStopped = false;
            if (agent.transform.position == target)
            {
                agent.speed /= 1.5f;
            }
        }
        else
        {
            yield return null;
            agent.destination = playerTransform.position;
        } 
    } 

    private void OnTriggerEnter(Collider other)
    {
        FPSInput playerFPS = other.GetComponent<FPSInput>();
        if (playerFPS != null) // check for player
        {
            playerFPS.TakeDamage(enemyScript.GetDmgPerHit());
        }
    }
}
