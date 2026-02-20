using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class MonsterAi : MonoBehaviour
{
    public FieldOfView fov;
    bool canSeePlayer;
    float distanceToPlayer;

    public enum EnemyState { Idle, Patrol, Chase, Attack }
    private EnemyState previousState;
    public EnemyState currentState;
    NavMeshAgent agent;
    private Vector3 patrolTarget;
    public bool isChasing = false;

    private float lastSeenTimer = 0f;
    public float memoryTime = 1f;

    public Transform player;

    public float chaseRange = 10f;
    public float attackRange = 2f;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;

    public float idleTime = 3f;
    private float idleTimer;

    bool isPatrolling = false;
    bool isIdling = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        previousState = currentState;

        StartIdle();
        fov = GetComponent<FieldOfView>();
    }

    void Update()
{
    canSeePlayer = fov.canSeePlayer;
    distanceToPlayer = fov.distanceToTarget;

    // 3️⃣ Hafıza timer (Memory) update
    if (canSeePlayer)
        lastSeenTimer = memoryTime;
    else
        lastSeenTimer -= Time.deltaTime;

    bool playerVisibleOrRemembered = lastSeenTimer > 0f;

    // 4️⃣ State belirleme
    if (playerVisibleOrRemembered && distanceToPlayer <= chaseRange)
    {
        currentState = EnemyState.Chase;
        isChasing = true;
    }
    else
    {
        // Görmüyorsa normal Idle/Patrol
        if (idleTimer <= 0)
        {
            currentState = EnemyState.Patrol;
            if (!isPatrolling)
                StartPatrol();
            isIdling = false;
            isChasing = false;
        }
        else
        {
            currentState = EnemyState.Idle;
            if (!isIdling)
                StartIdle();
            idleTimer -= Time.deltaTime;
            isPatrolling = false;
            isChasing = false;
        }
    }

    // 6️⃣ State davranışlarını çalıştır
    switch (currentState)
    {
        case EnemyState.Idle:
            IdleBehavior();
            break;
        case EnemyState.Patrol:
            PatrolBehavior();
            break;
        case EnemyState.Chase:
            ChasePlayer();
            break;
        case EnemyState.Attack:
            AttackPlayer();
            break;
    }
}

    
    void StartIdle()
    {
        idleTimer = Random.Range(10f, idleTime);
        isIdling = true;
        isPatrolling = false;
        agent.isStopped = true;
    }

    void IdleBehavior()
    {
        idleTimer -= Time.deltaTime;

        if (idleTimer <= 0f)
        {
            isIdling = false;
            StartPatrol();
        }
    }

    void ChasePlayer()
    {
        agent.isStopped = false;
        if (agent.speed != chaseSpeed) agent.speed = chaseSpeed;
        agent.SetDestination(player.position);
    }

    void AttackPlayer()
    {
        agent.isStopped = true;
        Debug.Log("ATTACK!");
    }

    void StartPatrol()
    {
        patrolTarget = GetRandomPoint(transform.position, 10f);
        agent.isStopped = false;
        agent.speed = patrolSpeed;
        agent.SetDestination(patrolTarget);
        isPatrolling = true;
    }


    void PatrolBehavior()
    {
        if (!agent.pathPending && agent.remainingDistance < 1f)
        {
            if (Random.value < 0.25f)
            {
                StartIdle();
            }
            else
            {
                StartPatrol();
            }
        }
    }

    Vector3 GetRandomPoint(Vector3 center, float range)
    {
        Vector3 randomPos = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPos, out hit, range, NavMesh.AllAreas);
        return hit.position;
    }
}