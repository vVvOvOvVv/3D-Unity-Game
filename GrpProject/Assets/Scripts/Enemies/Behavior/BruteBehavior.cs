using System.Collections;
using UnityEngine;
using UnityEngine.AI; // required for NavMesh

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent (typeof(CapsuleCollider))]
public class BruteBehavior : MonoBehaviour
{
    [SerializeField] private Transform playerTransform; // to allow AI to follow player
    [SerializeField] private Enemy enemyScript;
    NavMeshAgent agent;
    private GameObject player;

    private IEnumerator AgentNearPlayer()
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

    // when shock takes effect, enemy stays in place for 3 sec
    public IEnumerator ShockEnemy(int timeInSec)
    {
        agent.isStopped = true;
        yield return new WaitForSeconds(timeInSec);
        agent.isStopped = false;
    }

    // when poision is in effect, slow enemies
    public IEnumerator PoisonEnemy(int timeInSec, float spdFactor)
    {
        agent.speed *= spdFactor;
        yield return new WaitForSeconds(timeInSec);
        agent.speed /= spdFactor;
    }

    private void OnTriggerEnter(Collider other)
    {
        FPSInput playerFPS = other.GetComponent<FPSInput>();
        if (playerFPS != null) // check for player
        {
            playerFPS.TakeDamage(enemyScript.GetDmgPerHit());
        }
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
    }

    private void Update()
    { 
        StartCoroutine(AgentNearPlayer());
    }
}
