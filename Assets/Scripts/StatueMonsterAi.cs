using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class StatueMonsterAi : MonoBehaviour
{
    public Transform[] waypoints;
    int currentWaypoint = 0;

    FieldOfView fov;

    bool canSeePlayer;
    bool seenByPlayer;

    NavMeshAgent agent;

    public Transform player;

    public enum EnemyState {Idle, Patrol, Chase }
    public EnemyState currentState;

    float lastSeenTimer = 0f;
    public float memoryTime = 1f;
    float distanceToPlayer;

    public float chaseRange = 10f;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;

    public float idleTime = 3f;
    private float idleTimer;

    bool isPatrolling = false;
    bool isIdling = false;
    public bool isChasing = false;
    

    void Start()
    {
        fov = GetComponent<FieldOfView>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        canSeePlayer = fov.canSeePlayer;
        distanceToPlayer = fov.distanceToPlayer;

        if (CheckVisibility())
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            agent.updatePosition = false;
            agent.updateRotation = false;
            return;
        }
        else
        {
            agent.isStopped = false;
            agent.updatePosition = true;
            agent.updateRotation = true;
        }

        if (canSeePlayer)
        lastSeenTimer = memoryTime;
        else
        lastSeenTimer -= Time.deltaTime;

        bool playerVisibleOrRemembered = lastSeenTimer > 0f;

        if (playerVisibleOrRemembered && distanceToPlayer <= chaseRange)
        {
            currentState = EnemyState.Chase;
            isChasing = true;
        }
        else
        {
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
        }
    }

    void StartIdle()
    {
        idleTimer = Random.Range(0f, idleTime);
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

    void StartPatrol()
    {
        if (waypoints.Length == 0) return;

        currentWaypoint = Random.Range(0, waypoints.Length);
        agent.SetDestination(waypoints[currentWaypoint].position);

        agent.isStopped = false;
        agent.speed = patrolSpeed;
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

    bool CheckVisibility()
{
    Camera cam = Camera.main;
    
    Vector3 screenPoint = cam.WorldToViewportPoint(transform.position);
    bool inViewport = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

    if (!inViewport) return false;

    RaycastHit hit;
    
    Vector3 targetPoint = transform.position + Vector3.up * 1f; 
    Vector3 direction = targetPoint - cam.transform.position;

    if (Physics.Raycast(cam.transform.position, direction, out hit))
    {
        if (hit.transform == transform || hit.transform.IsChildOf(transform))
        {
            return true;
        }
    }
    return false;
}
}