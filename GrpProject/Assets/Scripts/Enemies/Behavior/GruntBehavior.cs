using System.Collections;
using UnityEngine;
using UnityEngine.AI; // required for NavMesh

[RequireComponent(typeof(NavMeshAgent))]
public class GruntBehavior : MonoBehaviour
{
    [SerializeField] private Transform playerTransform; // to allow AI to follow player
    [SerializeField] private Enemy enemyScript;
    [SerializeField] private GameObject projectilePrefab; // Prefab for the projectile
    [SerializeField] private Transform firePoint; // The point from where the projectile will be fired
    NavMeshAgent agent;
    private GameObject player;
    private bool isShooting = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");

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

    private void Update()
    {
        agent.destination = playerTransform.position; // agent walks towards player

        if (!isShooting)
        {
            StartCoroutine(AgentNearPlayer());
        }
    }

    private IEnumerator AgentNearPlayer()
    {
            isShooting = true;
            float distanceToPlayer = Vector3.Distance(agent.transform.position, playerTransform.position);
            if (distanceToPlayer <= 5.0f)
            {
                agent.isStopped = true;

                for (int i = 0; i < 3; i++) // Shoot 3 times
                {
                    Shoot();
                    yield return new WaitForSeconds(0.5f); // Wait between shots
                }

                yield return new WaitForSeconds(1); // Pause before moving again
                agent.isStopped = false;
            }
            else
            {
                // Continue following the player
                agent.isStopped = false;
                agent.destination = playerTransform.position;
                yield return null;
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
    public IEnumerator ShockEnemy(int timeInSec)
    {
        Debug.Log("Shock effect on: " + gameObject.name);
        agent.isStopped = true;
        yield return new WaitForSeconds(timeInSec);
        agent.isStopped = false;
        Debug.Log("Shock effect ended on: " + gameObject.name);
    }

    // when poison is in effect, slow enemies
    public IEnumerator PoisonEnemy(int timeInSec, float spdFactor)
    {
        agent.speed *= spdFactor;
        // dmg enemies over this time period
        yield return new WaitForSeconds(timeInSec);
        agent.speed /= spdFactor;
    }
}
