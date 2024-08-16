using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SniperBehavior : Behavior
{ 
    public int health = 10;
     
    private float shootInterval = 5.0f; // Time between shots
    private bool isShooting = false;
    private int currentWaypoint = 0;
    private bool isAiming = false;

    [SerializeField] private Transform[] waypoints;
    private LineRenderer lineRenderer;

    [SerializeField] private Transform firePoint; // The point from where the projectile will be fired 
    private new void Start()
    {
        base.Start();  
        //agent = GetComponent<NavMeshAgent>();

        // Create and configure the LineRenderer for the laser beam
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.001f;
        lineRenderer.endWidth = 0.001f;
        lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
        lineRenderer.material.color = Color.red;
        lineRenderer.positionCount = 2; // Line will always have two points (start and end)

        if (waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[currentWaypoint].position);
        }
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

    private new void Update()
    {
        if (waypoints.Length == 0 || playerTransform == null)
            return;

        // Always face the player
        /*Vector3 direction = (playerTransform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
*/
        if (!isAiming && !isShooting)
        {
            // Face the waypoint while walking
            Vector3 direction = (waypoints[currentWaypoint].position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
        else if (isAiming || isShooting)
        {
            // Face the player while aiming or shooting
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            Shoot();
        }

        // Update the laser beam's positions to follow the player
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, playerTransform.position);
        }

        if (!isShooting && !isAiming && Vector3.Distance(agent.transform.position, waypoints[currentWaypoint].position) < 2.0f)
        {
            StartCoroutine(AimAndShoot());
        }
        else if (!isAiming)
        {
            // Move to the next waypoint if the sniper is not aiming or shooting
            agent.SetDestination(waypoints[currentWaypoint].position);
        }
    }

    private IEnumerator AimAndShoot()
    {
        isAiming = true;
        agent.isStopped = true;

        // Transition to aim state
        enemyAnim.SetTrigger("walk to aim");

        // Wait for a moment to simulate aiming
        yield return new WaitForSeconds(2.0f);

        // Simulate shooting at the player
        enemyAnim.SetTrigger("aim to shoot");
         
        // fire at player
        // Debug.Log("Sniper fires!");
        isShooting = true; 

        // Wait before moving again
        yield return new WaitForSeconds(shootInterval);

        isShooting = false;
        isAiming = false;
        agent.isStopped = false;

        // Transition back to aim state
        enemyAnim.SetTrigger("shoot to aim");

        // Move to the next waypoint
        currentWaypoint++;
        if (currentWaypoint >= waypoints.Length)
        {
            currentWaypoint = 0; // Loop back to the first waypoint
        }
 
        enemyAnim.SetTrigger("aim to walk");
    }

    private void Shoot()
    {
        RaycastHit hit;  // Variable to store information about what the raycast hits.
        Vector3 directionToPlayer = (playerTransform.position - firePoint.position).normalized;
        // Calculate the direction from the fire point to the player's position and normalize it.

        if (Physics.Raycast(firePoint.position, directionToPlayer, out hit))
        {
            // Perform a raycast from the fire point in the direction of the player. 
            if (hit.collider.CompareTag("Player")) // Check if the raycast hit the player. 
            {   
                // make player take damage
                FPSInput fps = hit.collider.GetComponent<FPSInput>();
                fps.TakeDamage(enemyScript.dmgPerHit);
                Debug.Log(gameObject.name + " dealt " + enemyScript.dmgPerHit + " dmg!");
            }
        }
    }
}