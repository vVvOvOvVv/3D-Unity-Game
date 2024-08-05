using UnityEngine;
using UnityEngine.AI; // required for NavMesh

[RequireComponent(typeof(NavMeshAgent))]
public class GruntBehavior : MonoBehaviour
{
    [SerializeField] private Transform playerTransform; // to allow AI to follow player
    NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        agent.destination = playerTransform.position;
        // when the agent is about 5m away from the player, stop and beginning shooting
        // shoot 3 times, then move again
    }
}
