using UnityEngine;

public class MonsterAnimationHandler : MonoBehaviour
{
    public Animator anim;
    Rigidbody rb;
    float speed = 0f;
    MonsterAi.EnemyState state = MonsterAi.EnemyState.Idle;
    MonsterAi.EnemyState Chase = MonsterAi.EnemyState.Chase;
    Vector3 lastPos;

    void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
        state = gameObject.GetComponent<MonsterAi>().currentState;
    }

    void FixedUpdate()
    {
        speed = Vector3.Distance(transform.position, lastPos) / Time.deltaTime;
        lastPos = transform.position;
    }

    void Update()
    {
        state = gameObject.GetComponent<MonsterAi>().currentState;
        
        if (state == Chase)
        {
            anim.SetFloat("ChaseValue", 1f, 0.25f, Time.deltaTime);
        }else
        {
            anim.SetFloat("ChaseValue", 0f, 0.25f, Time.deltaTime);
        }

        anim.SetFloat("Speed", speed, 0.25f, Time.deltaTime);
    }
}