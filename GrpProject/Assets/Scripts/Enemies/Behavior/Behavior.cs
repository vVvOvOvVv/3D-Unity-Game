using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Behavior : MonoBehaviour
{

    [SerializeField] public Transform playerTransform; // to allow AI to follow player
    [SerializeField] public Enemy enemyScript;
    [SerializeField] public GameObject enemyModel; 
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
            agent.isStopped = true;
            yield return new WaitForSeconds(timeInSec);
            agent.isStopped = false;
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
            Debug.Log("Posion effect on: " + gameObject.name);
            agent.speed *= spdFactor;
            yield return new WaitForSeconds(timeInSec);
            agent.speed /= spdFactor;
            Debug.Log("Posion effect ended on: " + gameObject.name);
            isPoisoned = false;
        } 
    }

    protected void Start()
    {
        agent = GetComponent<NavMeshAgent>(); 
        player = GameObject.FindWithTag("Player");
        spd = agent.speed;
    }

    public void Update()
    {
        if (agent != null)
        {
            StartCoroutine(AgentNearPlayer());
        }
    }
}
