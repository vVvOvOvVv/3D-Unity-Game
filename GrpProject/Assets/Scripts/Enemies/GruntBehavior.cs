using System.Collections;
using UnityEngine;
using UnityEngine.AI; // required for NavMesh

[RequireComponent(typeof(NavMeshAgent))]
public class GruntBehavior : MonoBehaviour
{
    [SerializeField] private Transform playerTransform; // to allow AI to follow player
    NavMeshAgent agent; 

    private IEnumerator AgentNearPlayer()
    {
        // when the agent is about 5m away from the player, stop and beginning shooting
        // shoot 3 times, then move again 
        float distanceToPlayer = Vector3.Distance(agent.transform.position, playerTransform.position);
        if (distanceToPlayer <= 5.0f)
        {
            agent.isStopped = true;
            // fire gun three times
            yield return new WaitForSeconds(1);
            agent.isStopped = false;
        }
        else yield return null;
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        agent.destination = playerTransform.position; // agent walks towards player

        StartCoroutine(AgentNearPlayer());
    }
}
