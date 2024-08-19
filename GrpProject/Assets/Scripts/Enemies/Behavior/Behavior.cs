using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Behavior : MonoBehaviour
{

    [SerializeField] public Transform playerTransform; // to allow AI to follow player
    [SerializeField] public Enemy enemyScript;
    [SerializeField] public GameObject enemyModel,
        poisonParticlePrefab, shockParticlePrefab; // status ailment indicators 
    public enum EnemyState { Move, Idle, Attack};
    public EnemyState enemyState = EnemyState.Move;
    public Animator enemyAnim;
    public float spd;

    public NavMeshAgent agent;
    public GameObject player;
    public bool isShocked, isPoisoned, isBurn; 

    public virtual IEnumerator AgentNearPlayer() { yield return null; }

    // when shock takes effect, enemy stays in place for 3 sec (just added debuglogs to test shock effect)
    public virtual IEnumerator ShockEnemy(int timeInSec)
    {
        if (!isShocked)
        {
            isShocked = true;
            Debug.Log("Shock effect on: " + gameObject.name);
            GameObject shock = Instantiate(shockParticlePrefab, transform);
            agent.isStopped = true;
            yield return new WaitForSeconds(timeInSec);
            agent.isStopped = false;
            Destroy(shock);
            Debug.Log("Shock effect ended on: " + gameObject.name);
            isShocked = false;
        }

    }

    // when poision is in effect, slow enemies
    public virtual IEnumerator PoisonEnemy(int timeInSec, float spdFactor)
    { 
        if (!isPoisoned)
        {
            isPoisoned = true;
            Debug.Log("Poison effect on: " + gameObject.name);
            GameObject poison = Instantiate(poisonParticlePrefab, transform);
            agent.speed *= spdFactor;
            yield return new WaitForSeconds(timeInSec);
            agent.speed /= spdFactor;
            Destroy(poison);
            Debug.Log("Poison effect ended on: " + gameObject.name);
            isPoisoned = false;
        } 
    }

    public void Start()
    {
        agent = GetComponent<NavMeshAgent>(); 
        player = GameObject.FindWithTag("Player");
        spd = agent.speed;
    }

    public void Update()
    {
        // ensures ai status is NOT shocked then starts AgentNearPlayer back up
        if (agent != null && !isShocked)
        {
            StartCoroutine(AgentNearPlayer());
        }
    }
}
