using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Boss))]
public class BossBehavior : Behavior
{
    // bools
    public bool phase2,  // determine phase of the fight
        isAttacking, // to prevent repeated attacks when not intended
        playerInRoom; // determine if player is in the room - begin fight 
    private bool isRunning, // prevent repetitive animation of run
        alternateFlag;
    private Boss bossScript;
    [HideInInspector] public AudioSource roar;

    // attack colliders
    [SerializeField]
    public GameObject swipeHitBox, swipeRange,
        jumpHitBox, jumpRange, jumpColliders;

    private new void Start()
    {
        base.Start(); // call parent Start()  

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        bossScript = GetComponent<Boss>();  
        roar = GetComponent<AudioSource>();

        alternateFlag = true;
        // phase2 = false;
        isRunning = false;
        isAttacking = false;
        jumpColliders.SetActive(false);
    }

    // Update is called once per frame
    private new void Update()
    {
        if (agent != null && !isShocked && !bossScript.isDead)
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

            if (playerInRoom && !isAttacking) // only start phases if player is in the room
            {
                if (phase2)
                    PhaseTwo();
                else // phase 1
                    PhaseOne();
            }

            yield return null; // wait for next frame
        }
    } 

    public IEnumerator SwipeAttack()
    {
        isAttacking = true;
        agent.destination = agent.transform.position; // stop agent from moving
        enemyAnim.SetTrigger("Swipe");

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
            yield return new WaitForSeconds(1.21f);
            roar.Play();
            yield return new WaitForSeconds(4.03f);
        }

        alternateFlag = !alternateFlag; // alternate between two animations
        isAttacking = false;
        isRunning = false; // allow for animation trigger call
    }

    private void PhaseOne()
    {
        agent.speed = 10f; // increase max speed to 10f 
        agent.destination = player.transform.position;

        if (!isRunning) // ensure call for running animation only happens once each "cycle"
        {
            isRunning = true;
            enemyAnim.SetTrigger("Run");
        }
    }

    public IEnumerator JumpAttack()
    {
        if (isAttacking) yield break; // prevent overlapping jump attacks
        isAttacking = true;
        agent.destination = agent.transform.position; // stop for a moment
        // Debug.Log("Boss will jump!");
        enemyAnim.SetTrigger("Jump Attack");
        yield return new WaitForSeconds(0.1f);

        agent.speed = 30f; // max speed to 30f
        agent.destination = player.transform.position;
        // Debug.Log("Boss landed!");
        yield return new WaitForSeconds(1.1f);

        // Debug.Log("Boss finished the jump attack!"); 
        agent.destination = agent.transform.position; // stop in place to prevent "sliding"
        jumpHitBox.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        jumpHitBox.SetActive(false);

        yield return new WaitForSeconds(2.1f); // wait for end of animation 

        enemyAnim.SetTrigger("Roar"); // taunt
        yield return new WaitForSeconds(1.21f);
        roar.Play();
        yield return new WaitForSeconds(4.03f);

        isAttacking = false;
        isRunning = false; // allow for animation trigger call

        yield return new WaitForSeconds(2f); // Cooldown to prevent immediate jump
    }

    private void PhaseTwo()
    { 
        if (!isAttacking)
        {
            float distanceToPlayer = Vector3.Distance(agent.transform.position, player.transform.position);
            float jumpRangeRadius = jumpRange.GetComponent<SphereCollider>().radius;
            WithinBossRange withinBossRange = jumpRange.GetComponent<WithinBossRange>();

            // Debug.Log($"[PhaseTwo] Distance to player: {distanceToPlayer}, Jump Range: {jumpRangeRadius}");
            // Debug.Log($"[PhaseTwo] IsAttacking: {isAttacking}, IsRunning: {isRunning}"); 

            agent.speed = 20f;
            agent.destination = player.transform.position;
            if (!isRunning) // to prevent repeated calls to SetTrigger
            {
                // Debug.Log("[PhaseTwo] Player is out of jump range or jump is disabled. Boss should start running.");
                isRunning = true;
                enemyAnim.SetTrigger("Run");
            } 
            // swipe attack still active -  will attack when in range
        } /*
        else
        {
            Debug.Log("[PhaseTwo] Boss is currently attacking, skipping logic.");
        } */
    }


}