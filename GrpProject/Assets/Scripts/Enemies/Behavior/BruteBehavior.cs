using System.Collections;
using UnityEngine;
using UnityEngine.AI; // required for NavMesh

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent (typeof(CapsuleCollider))]
public class BruteBehavior : Behavior
{
    private bool canAttack;
    private float originalSpeed; // FOR STORING ORIGINAL SPEED TO RESET TO AFTER POISON EFFECT ENDS

    private new void Start()
    {
        base.Start(); 
        canAttack = true;
        originalSpeed = spd; // initialize original speed
    }

    public override IEnumerator AgentNearPlayer()
    {
        while (true)
        {
            // SHOCK LOGIC CHECK FOR SHOCKED STATUS BEFORE MOVING AGAIN (AgentNearPlayer)
            if (isShocked)
            {
                agent.isStopped = true;
                agent.speed = 0;
                yield return null;
                continue;
            }

            // POISON LOGIC CHECK FOR POISONED STATUS (AgentNearPlayer)
            if (isPoisoned)
            {
                agent.speed = originalSpeed * 0.5f; // reduce speed to half when poisoned
                //Debug.Log("AI Entity is poisoned. Speed reduced to: " + agent.speed); // DEBUG LOG FOR POISON STATUS
            }
            else
            {
                agent.speed = originalSpeed; // reset speed to normal if not poisoned
                //Debug.Log("AI Entity is no longer poisoned. Speed back to: " + agent.speed); // DEBUG LOG FOR POISON STATUS
            }

            // Calculate the distance to the player
            float distanceToPlayer = Vector3.Distance(agent.transform.position, playerTransform.position);

            if (distanceToPlayer <= 15.0f && agent != null && canAttack)
            {
                // Pause and mark the player's position
                canAttack = false;
                agent.speed = 0;
                agent.isStopped = true; // Stop the agent
                enemyAnim.SetTrigger("Move to Idle");

                Vector3 target = playerTransform.position; // Mark the player's current position

                yield return new WaitForSeconds(1); // Wait for 1 second

                // Charge towards the marked position at triple speed
                agent.speed = spd * 3f;
                agent.isStopped = false; // Resume agent movement
                agent.destination = target;
                enemyAnim.SetTrigger("Idle to Attack");

                // Wait until the agent reaches the target or the player moves too far away
                while (Vector3.Distance(agent.transform.position, target) > agent.stoppingDistance)
                {
                    distanceToPlayer = Vector3.Distance(agent.transform.position, playerTransform.position);

                    // If the player moves more than 15 units away from the target, break and chase the player
                    if (distanceToPlayer > 15.0f) 
                        break; 

                    yield return null;
                }

                // Pause for 2 seconds at the target position if the player is still close
                if (Vector3.Distance(agent.transform.position, target) <= agent.stoppingDistance)
                {
                    agent.speed = 0;
                    agent.isStopped = true; // Stop the agent
                    enemyAnim.SetTrigger("Attack to Idle"); 

                    yield return new WaitForSeconds(2);
                }

                // Resume walking towards the player
                agent.speed = spd;
                agent.isStopped = false; 
                enemyAnim.SetTrigger("Idle to Move");
                canAttack = true;
            }
            else
            {
                // Continue following the player if they are too far away
                agent.isStopped = false;
                agent.speed = spd;
                agent.destination = playerTransform.position;
                canAttack = true;
            }

            yield return null; // Wait until the next frame before continuing the loop
        }
    }


    /* private void OnTriggerEnter(Collider other)
    {
        FPSInput playerFPS = other.GetComponent<FPSInput>();
        if (playerFPS != null) // check for player
        {
            playerFPS.TakeDamage(enemyScript.GetDmgPerHit());
            Debug.Log("You took damage!");
        }
    } */

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")) // check for player
        {
            FPSInput playerFPS = collision.gameObject.GetComponent<FPSInput>();

            playerFPS.TakeDamage(enemyScript.GetDmgPerHit());
            Debug.Log("You took damage!");
        } 
    }
}
