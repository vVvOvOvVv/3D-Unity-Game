using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class GruntBehavior : Behavior 
{ 
    [SerializeField] private GameObject projectilePrefab; // Prefab for the projectile
    [SerializeField] private Transform firePoint; // The point from where the projectile will be fired 
     
    private bool canAttack = true;
  
    private new void Start()
    {
        base.Start();

        if (player != null)
        {
            playerTransform = player.transform;
        }

        if (firePoint == null)
        {
            firePoint = transform.Find("Fire Point");
        }
    }

    /*private new void Update() 
    {
        agent.destination = playerTransform.position;

        if (!isShooting)
        {
            StartCoroutine(AgentNearPlayer());
        }
    }*/

    public override IEnumerator AgentNearPlayer()
    {
        while (true)
        {
            float distanceToPlayer = Vector3.Distance(agent.transform.position, playerTransform.position);

            if (distanceToPlayer > 8.0f)
            {
                // Move towards the player if further than the attack distance
                agent.isStopped = false;
                agent.speed = spd;
                canAttack = true;
                agent.destination = playerTransform.position;

                if (!agent.isStopped && agent.velocity.magnitude > 0.1f)
                {
                    enemyAnim.SetTrigger("aim to walk"); // Play walk animation
                }
            }
            else if (distanceToPlayer <= 8.0f && canAttack)
            {
                canAttack = false;
                agent.isStopped = true; // stop in place

                // Perform the shooting action three times
                for (int i = 0; i < 3; i++)
                {
                    Shoot();
                    yield return new WaitForSeconds(0.5f); // Wait between shots
                }

                // Resume movement after attacking
                enemyAnim.SetTrigger("shoot to aim");
                yield return new WaitForSeconds(1.0f); // Wait before the next attack phase

                // Recalculate distance to player to decide further action
                distanceToPlayer = Vector3.Distance(agent.transform.position, playerTransform.position);

                if (distanceToPlayer <= 8.0f)
                {
                    agent.isStopped = true;
                    agent.velocity = Vector3.zero;
                    enemyAnim.SetTrigger("aim to walk"); // Keep animation consistent
                }
                else
                {
                    agent.isStopped = false;
                    yield return new WaitForSeconds(0.5f); // Optional: small delay to prevent immediate r
                    canAttack = true; // Allow further attacks if player moves away
                }
            } 
            yield return null; // Wait until the next frame
        }
    }


    private void Shoot()
    {
        RaycastHit hit;  // Variable to store information about what the raycast hits.
        Vector3 directionToPlayer = (playerTransform.position - firePoint.position).normalized;
        // Calculate the direction from the fire point to the player's position and normalize it.

        if (Physics.Raycast(firePoint.position, directionToPlayer, out hit))
        {
            // Perform a raycast from the fire point in the direction of the player.

            if (hit.collider.CompareTag("Player"))
            {
                // Check if the raycast hit the player.

                GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
                // Instantiate a projectile at the fire point's position with its rotation.

                projectile.transform.forward = directionToPlayer;
                // Set the projectile's forward direction towards the player.

                Projectile projScript = projectile.GetComponent<Projectile>();
                if (projScript != null)
                {
                    projScript.SetDirection(directionToPlayer);
                    // Set the projectile's movement direction if the script is attached.
                }

                // make player take damage
                FPSInput fps = hit.collider.GetComponent<FPSInput>();
                fps.TakeDamage(enemyScript.dmgPerHit);
                Debug.Log(gameObject.name + " dealt " + enemyScript.dmgPerHit + " dmg!");
            }
        }
    } 
}
