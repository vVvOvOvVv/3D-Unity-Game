using System.Collections;
using UnityEngine;
using UnityEngine.AI; // required for NavMesh

[RequireComponent(typeof(NavMeshAgent))]
public class GruntBehavior : Behavior 
{
    [SerializeField] private GameObject projectilePrefab; // Prefab for the projectile
    [SerializeField] private Transform firePoint; // The point from where the projectile will be fired

    //private bool isShooting = false;
    private bool canAttack = true;
  
    private new void Start()
    {
        base.Start();
        StartCoroutine(AgentNearPlayer()); 

        if (player != null)
        {
            playerTransform = player.transform;
        }

        // Ensure firePoint is set; alternatively, you can manually set it in the Unity editor
        if (firePoint == null)
        {
            firePoint = transform.Find("FirePoint"); // Assuming there's a child object named FirePoint
        }
    }
 
    /*private new void Update() 
    {
        agent.destination = playerTransform.position; // agent walks towards player

        if (!isShooting)
        {
            StartCoroutine(AgentNearPlayer());
        }
    }*/
 
    public override IEnumerator AgentNearPlayer()
    {
        //isShooting = true; 
        while (true)
        {
            float distanceToPlayer = Vector3.Distance(agent.transform.position, playerTransform.position);

            if (distanceToPlayer > 5.0f)
            {
                
                // Move towards the player if further than the attack distance
                agent.isStopped = false;
                agent.speed = spd;
                agent.destination = playerTransform.position;
                // Move towards the player if further than the attack distance
                if (!agent.isStopped && agent.velocity.magnitude > 0.1f)
                {
                    enemyAnim.SetTrigger("aim to walk"); // Play walk animation
                }

                // Set the animation to walk
                //enemyAnim.SetTrigger("aim to walk");
            }
            else if (distanceToPlayer <= 5.0f && canAttack)
            {
                // Stop moving and start aiming
                agent.isStopped = true;
                agent.speed = 0;
                enemyAnim.SetTrigger("walk to aim");

                yield return new WaitForSeconds(1.0f); // Simulate aiming

                // Start attacking
                canAttack = false;
                enemyAnim.SetTrigger("aim to shoot");

                // Shoot projectiles
                for (int i = 0; i < 3; i++) // Shoot 3 times
                {
                    Shoot();
                    yield return new WaitForSeconds(0.5f); // Wait between shots
                }

                // Resume movement after attacking
                enemyAnim.SetTrigger("shoot to aim");
                /*agent.isStopped = false;
                agent.speed = spd;
                enemyAnim.SetTrigger("shoot to aim");*/
                
                //agent.destination = playerTransform.position;
                //enemyAnim.SetTrigger("aim to walk");
                // Wait before the next attack phase
                yield return new WaitForSeconds(2.0f);
                canAttack = true;
            }

            // Smoothly stop the agent if too close to the player
            if (distanceToPlayer <= 5.0f) 
            {
                if (!agent.isStopped)
                {
                    agent.isStopped = true;
                    agent.velocity = Vector3.zero;
                    enemyAnim.SetTrigger("aim to walk"); // Keep animation consistent
                }
            }

            yield return null; // Wait until the next frame
        }
    } 

    private void Shoot()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            // Instantiate the projectile at the firePoint position
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

            // Calculate the direction towards the player
            Vector3 directionToPlayer = (playerTransform.position - firePoint.position).normalized;

            // Rotate the projectile to face the player
            projectile.transform.forward = directionToPlayer;

            // Get the projectile script and set its direction
            Projectile projScript = projectile.GetComponent<Projectile>();
            if (projScript != null)
            {
                projScript.SetDirection(directionToPlayer);
            }

            Debug.Log("Grunt fired a projectile.");
        }
        else
        {
            Debug.LogError("Projectile prefab or FirePoint is not assigned.");
        }
    }

    // when shock takes effect, enemy stays in place for 3 sec 
    public override IEnumerator ShockEnemy(int timeInSec) 
    public new IEnumerator ShockEnemy(int timeInSec) 
    {
        Debug.Log("Shock effect on: " + gameObject.name);
        agent.isStopped = true;
        yield return new WaitForSeconds(timeInSec);
        agent.isStopped = false;
        Debug.Log("Shock effect ended on: " + gameObject.name);
    }

    // when poison is in effect, slow enemies 
    public override IEnumerator PoisonEnemy(int timeInSec, float spdFactor) 
    public new IEnumerator PoisonEnemy(int timeInSec, float spdFactor) 
    {
        agent.speed *= spdFactor;
        // dmg enemies over this time period
        yield return new WaitForSeconds(timeInSec);
        agent.speed /= spdFactor;
    }
}
