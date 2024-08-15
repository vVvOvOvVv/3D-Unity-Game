using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class GruntBehavior : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
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

        if (firePoint == null)
        {
            firePoint = transform.Find("FirePoint");
        }
    }

    private void Update()
    {
        agent.destination = playerTransform.position;

        if (!isShooting)
        {
            StartCoroutine(AgentNearPlayer());
        }
    }

    private IEnumerator AgentNearPlayer()
    {
        isShooting = true;
        float distanceToPlayer = Vector3.Distance(agent.transform.position, playerTransform.position);

        if (distanceToPlayer <= 8.0f)
        {
            agent.isStopped = true;

            for (int i = 0; i < 3; i++)
            {
                Shoot();
                yield return new WaitForSeconds(0.5f);
            }

            yield return new WaitForSeconds(1);
            agent.isStopped = false;
        }
        else
        {
            agent.isStopped = false;
            yield return null;
        }

        isShooting = false;
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
            }
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
