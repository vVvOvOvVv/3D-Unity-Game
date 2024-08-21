using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.FilePathAttribute;

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
            float distanceToPlayer = Vector3.Distance(agent.transform.position, player.transform.position);

            if (distanceToPlayer > 8.0f)
            {
                // Move towards the player if further than the attack distance
                agent.isStopped = false;
                agent.speed = spd;
                canAttack = true;
                agent.destination = player.transform.position;

                if (!agent.isStopped && agent.velocity.magnitude > 0.1f)
                {
                    enemyAnim.SetTrigger("shoot to walk"); // Play walk animation
                }
            }
            else if (distanceToPlayer <= 8.0f && canAttack)
            {
                canAttack = false;
                agent.isStopped = true; // stop in place
                enemyAnim.SetTrigger("walk to aim"); // Play walk animation

                // make agent look at player
                Vector3 dir = player.transform.position - transform.position;
                Quaternion lookRotation = Quaternion.LookRotation(dir);
                Vector3 rotation = lookRotation.eulerAngles;
                agent.transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);

                // Perform the shooting action three times
                StartCoroutine(Shoot());

                // Resume movement after attacking
                enemyAnim.SetTrigger("aim to shoot");
                yield return new WaitForSeconds(1.0f); // Wait before the next attack phase 

                agent.isStopped = false; 
                canAttack = true;
            } 
            yield return null; // Wait until the next frame
        }
    }


    private IEnumerator Shoot()
    {
        for (int i = 0; i < 3; i++)
        {
            RaycastHit hit;  // Variable to store information about what the raycast hits.
            Vector3 directionToPlayer = (player.transform.position - firePoint.position).normalized;
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

            yield return new WaitForSeconds(0.5f); // Wait between shots
        }
    }
}
