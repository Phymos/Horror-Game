using System.Collections;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAi : MonoBehaviour
{
    public enum EnemyState { Idle, Patrol, Chase, Attack }
    private EnemyState previousState;
    public EnemyState currentState;
    NavMeshAgent agent;
    private Vector3 patrolTarget;
    public Animator anim;

    [Header("Field of View Settings")]
    public Transform eyes;
    public LayerMask obstacleMask;
    public float viewAngle = 90f;
    public float viewDistance = 10f;
    private bool canSeePlayer = false;
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
        SetAnimationState(currentState);
    }

    void Update()
{
    // 1️⃣ Field of View kontrolü
    FieldOfViewCheck();

    // 2️⃣ Mesafeyi hesapla
    float distanceToPlayer = Vector3.Distance(transform.position, player.position);

    // 3️⃣ Hafıza timer (Memory) update
    if (canSeePlayer)
        lastSeenTimer = memoryTime;
    else
        lastSeenTimer -= Time.deltaTime;

    bool playerVisibleOrRemembered = lastSeenTimer > 0f;

    // 4️⃣ State belirleme
    if (playerVisibleOrRemembered && distanceToPlayer <= attackRange)
    {
        currentState = EnemyState.Attack;
    }
    else if (playerVisibleOrRemembered && distanceToPlayer <= chaseRange)
    {
        currentState = EnemyState.Chase;
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
        }
        else
        {
            currentState = EnemyState.Idle;
            if (!isIdling)
                StartIdle();
            idleTimer -= Time.deltaTime;
            isPatrolling = false;
        }
    }

    // 5️⃣ Animasyon değişimi kontrolü
    if (currentState != previousState)
    {
        SetAnimationState(currentState);
        previousState = currentState;
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

        int variant = Random.Range(0, 2);
        anim.SetInteger("idleVariant", variant);
        SetAnimationState(EnemyState.Idle);
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

        SetAnimationState(EnemyState.Chase);
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

        SetAnimationState(EnemyState.Patrol);
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

    void SetAnimationState(EnemyState newState)
    {
        anim.SetInteger("state", (int)newState);

        if (newState == EnemyState.Idle)
        {
            int variant = Random.Range(0, 2);
            anim.SetInteger("idleVariant", variant);
        }
    }

    void FieldOfViewCheck()
    {
        canSeePlayer = false;

        float distToPlayer = Vector3.Distance(eyes.position, player.position);

        // Çok yakınsa direkt gör
        if (distToPlayer < 2f)
        {
            canSeePlayer = true;
            return;
        }

        // Oyuncuya doğru yön
        Vector3 dirToPlayer = (player.position - eyes.position).normalized;

        // Görüş açısı kontrolü
        if (Vector3.Angle(eyes.forward, dirToPlayer) < viewAngle / 2f)
        {
            // Engel kontrolü (örneğin duvar)
            if (!Physics.Raycast(eyes.position, dirToPlayer, distToPlayer, obstacleMask))
            {
                if (distToPlayer <= viewDistance)
                    canSeePlayer = true;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (eyes == null) return;

        // Çizim rengi (sarı)
        Gizmos.color = Color.yellow;

        // Görüş mesafesi çemberi
        Gizmos.DrawWireSphere(eyes.position, viewDistance);

        // Sağ ve sol sınır yönleri
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2f, 0) * eyes.forward;
        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2f, 0) * eyes.forward;

        // Bu yönleri çiz
        Gizmos.DrawLine(eyes.position, eyes.position + rightBoundary * viewDistance);
        Gizmos.DrawLine(eyes.position, eyes.position + leftBoundary * viewDistance);

        // Ortadaki ileri yönü de (gri çizgiyle)
        Gizmos.color = Color.gray;
        Gizmos.DrawLine(eyes.position, eyes.position + eyes.forward * viewDistance);
    }
}
