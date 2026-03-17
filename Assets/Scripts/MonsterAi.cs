using UnityEngine;
using UnityEngine.AI;

public class MonsterAi : MonoBehaviour
{
    [SerializeField] float minDoorOpenTimer = 1f;
    [SerializeField] float maxDoorOpenTimer = 3f;
    float doorOpenTimer;
    public DoorInteract currentDoor;
    bool isWaitingForDoor = false;

    public FieldOfView fov;
    bool canSeePlayer;
    float distanceToPlayer;

    public enum EnemyState { Idle, Patrol, Chase}
    public EnemyState currentState;

    public Transform[] waypoints;
    int currentWaypoint = 0;

    NavMeshAgent agent;

    private Vector3 patrolTarget;
    public bool isChasing = false;

    private float lastSeenTimer = 0f;
    public float memoryTime = 1f;

    public Transform player;

    public float chaseRange = 10f;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;

    public float idleTime = 3f;
    private float idleTimer;

    bool isPatrolling = false;
    bool isIdling = false;

    [SerializeField] float hearingRange = 10f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        StartIdle();
        fov = GetComponent<FieldOfView>();
    }

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (currentDoor == null && isWaitingForDoor)
        {
            isWaitingForDoor = false;
            agent.isStopped = false;
        }

        if (currentDoor != null)
        DoorOpening();

        if (isWaitingForDoor) return;

        canSeePlayer = fov.canSeePlayer;
        distanceToPlayer = fov.distanceToPlayer;

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
 
    private void OnEnable() 
    {
        NoiseSystem.OnNoiseMade += OnHearedSound;
    }

    private void OnDisable() 
    {
        NoiseSystem.OnNoiseMade -= OnHearedSound;
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

    Vector3 GetRandomPoint(Vector3 center, float range)
    {
        Vector3 randomPos = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPos, out hit, range, NavMesh.AllAreas);
        return hit.position;
    }

    void DoorOpening()
    {
        Vector3 toDestination = agent.destination - transform.position;
        Vector3 toDoor = currentDoor.transform.position - transform.position;

        float dot = Vector3.Dot(toDestination.normalized, toDoor.normalized); //dot product (yönü bulmak için, 1 ise aynı yön -1 ise zıt)
        Debug.Log("dot: " + dot + " | isOpen: " + currentDoor.isOpen + " | isWaiting: " + isWaitingForDoor + " | timer: " + doorOpenTimer);

        if (currentDoor.isOpen)
        {
            agent.isStopped = false;
            isWaitingForDoor = false;
            currentDoor = null;
        }
    
        if (!isWaitingForDoor)
        {
            isWaitingForDoor = true;
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            doorOpenTimer = Random.Range(minDoorOpenTimer, maxDoorOpenTimer);
        }

        doorOpenTimer -= Time.deltaTime;
        if (doorOpenTimer <= 0f)
        {
            currentDoor.doorOpenClose();
            agent.isStopped = false;
            isWaitingForDoor = false;
            currentDoor = null;
        }
        
    }

    void OnHearedSound(Vector3 soundPos, float intensity)
    {
        float dist = Vector3.Distance(transform.position, soundPos);
        if (dist < intensity * hearingRange)
        {
            agent.SetDestination(soundPos);
        }
    }
}