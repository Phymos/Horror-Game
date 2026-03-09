using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class StatueMonsterAi : MonoBehaviour
{
    [SerializeField] float minDoorOpenTimer = 1f;
    [SerializeField] float maxDoorOpenTimer = 3f;
    float doorOpenTimer;
    public DoorInteract currentDoor;
    bool isWaitingForDoor = false;

    public Transform[] waypoints;
    int currentWaypoint = 0;

    FieldOfView fov;

    public bool isSeen;

    bool canSeePlayer;

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

        if (CheckVisibility())
        {
            isSeen = true;
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            agent.updatePosition = false;
            agent.updateRotation = false;
            return;
        }
        else
        {
            isSeen = false;
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

    Renderer[] renderers = GetComponentsInChildren<Renderer>();

    Bounds combinedBounds = renderers[0].bounds;
    foreach (Renderer rend in renderers)
        {
            combinedBounds.Encapsulate(rend.bounds);
        }

    Vector3 center = combinedBounds.center;
    
    Vector3[] bodyPoints = new Vector3[]
    {
        center,
        center + Vector3.up * combinedBounds.extents.y * 0.8f,
        center - Vector3.up * combinedBounds.extents.y,
    };
    
    foreach (Vector3 point in bodyPoints)
    {
        Vector3 screenPoint = cam.WorldToViewportPoint(point);
        bool inViewport = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

        if (!inViewport) continue;

        RaycastHit hit;
        
        Vector3 stableOrigin = cam.transform.parent != null ? cam.transform.parent.position : cam.transform.position;
        Vector3 direction = point - stableOrigin;

        if (Physics.Raycast(stableOrigin, direction, out hit))
        {
            if (hit.transform.root == transform.root)
            {
                return true;
            }
        }
    }
    return false;
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
}