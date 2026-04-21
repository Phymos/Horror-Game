using UnityEngine;
using UnityEngine.AI;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] DoorInteract door;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Monster")) return;

        StatueMonsterAi statueAi = other.GetComponent<StatueMonsterAi>();

        if (statueAi == null) return;
        if (door.isOpen == true)
        {
            statueAi.currentDoor = null;
            return;
        }
        
        statueAi.currentDoor = door;

        NavMeshAgent agent = other.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }

        MonsterAi monsterAi = other.GetComponent<MonsterAi>();
        if (monsterAi != null)
        {
            monsterAi.currentDoor = door;
        }
    }
}