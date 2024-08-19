using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Boss))]
public class BossBehavior : Behavior
{
    // attack-related variables
    [SerializeField] private Transform firePoint;
    [SerializeField]
    private float jumpAtkRange = 50f,
        runSpeed = 5f,
        swipeRange = 5f;

    // bools
    public bool phase2, phase3, // determine phase of the fight
        canAttack, // to prevent repeated attacks when not intended
        playerInRoom; // determine if player is in the room - begin fight 
    private bool posMarked, // prevent repetitive marking of player position
        alternateFlag;

    // enum variables
    public new enum EnemyState
    {
        Idle,
        Roar,
        Run,
        SwipeAttack,
        JumpAttack,
        Flex,
        Death
    }
    public EnemyState state;

    // attack colliders
    [SerializeField] private GameObject swipeHitBox;

    private new void Start()
    {
        base.Start(); // call parent Start() 

        state = EnemyState.Idle;

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
        if (firePoint == null)
            firePoint = transform.Find("FirePoint");

        alternateFlag = true;
        phase2 = false;
        phase3 = false;
        posMarked = false;
        canAttack = true;
    }

    // Update is called once per frame
    private new void Update()
    {
        if (agent != null && !isShocked)
            StartCoroutine(BehaviorHandler());
    }

    private IEnumerator BehaviorHandler()
    {
        while (true)
        {
            // SHOCK LOGIC CHECK FOR SHOCKED STATUS BEFORE MOVING AGAIN (AgentNearPlayer)
            if (isShocked)
            {
                agent.isStopped = true;
                agent.speed = 0;
                yield return null;
                continue;
            }

            // POISON LOGIC CHECK FOR POISONED STATUS (AgentNearPlayer)
            if (isPoisoned)
            {
                agent.speed = spd * 0.5f; // reduce speed to half when poisoned
                //Debug.Log("AI Entity is poisoned. Speed reduced to: " + agent.speed); // DEBUG LOG FOR POISON STATUS
            }
            else
            {
                agent.speed = spd; // reset speed to normal if not poisoned
                //Debug.Log("AI Entity is no longer poisoned. Speed back to: " + agent.speed); // DEBUG LOG FOR POISON STATUS
            }

            if (playerInRoom) // only start phases if player is in the room
            {
                if (phase3)
                {

                }
                else if (phase2)
                {

                }
                else // phase 1
                    PhaseOne();
            }

            yield return null; // wait for next frame
        }
    }

    public void AnimationHandler()
    {
        switch (state)
        {
            case EnemyState.Roar:
                enemyAnim.SetTrigger("Roar");
                break;
            case EnemyState.Run:
                enemyAnim.SetTrigger("Run");
                break;
            case EnemyState.SwipeAttack:
                enemyAnim.SetTrigger("Swipe");
                break;
            case EnemyState.JumpAttack:
                enemyAnim.SetTrigger("Jump Attack");
                break;
            case EnemyState.Flex:
                enemyAnim.SetTrigger("Flex");
                break;
            default: // idle
                enemyAnim.SetTrigger("Idle");
                break;
        }
    }

    private IEnumerator SwipeAttack()
    {
        agent.speed = spd;
        canAttack = false;
        enemyAnim.SetTrigger("Swipe");
        //AnimationHandler();

        yield return new WaitForSeconds(1.4f);
        swipeHitBox.SetActive(true); // make hitbox appear

        yield return new WaitForSeconds(0.6f);
        swipeHitBox.SetActive(false); // disable hitbox 

        if (alternateFlag)
        {
            enemyAnim.SetTrigger("Flex");
            yield return new WaitForSeconds(4.7f);
        }
        else
        {
            enemyAnim.SetTrigger("Roar");
            yield return new WaitForSeconds(5.4f);
        }

        alternateFlag = !alternateFlag; 
        posMarked = false; // allow for attack again
        canAttack = true;
    }

    private void PhaseOne()
    { 
        if (!posMarked) // prevent repetitive marking of player position
        {
            Debug.Log("Location marked!");
            playerTransform = player.transform;
            agent.destination = playerTransform.position;

            agent.speed *= runSpeed;
            enemyAnim.SetTrigger("Run");
            AnimationHandler();
            posMarked = true;
        }

        if (Vector2.Distance(agent.transform.position, playerTransform.position) <= swipeRange
            && canAttack)
            StartCoroutine(SwipeAttack());
    }
} 