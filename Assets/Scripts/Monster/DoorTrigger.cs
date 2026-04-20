using UnityEngine;
using UnityEngine.AI;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] DoorInteract door;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Monster")) return;

        StatueMonsterAi monsterAi = other.GetComponent<StatueMonsterAi>();

        if (monsterAi == null) return;
        if (door.isOpen == true)
        {
            monsterAi.currentDoor = null;
            return;
        }
        
        monsterAi.currentDoor = door;

        NavMeshAgent agent = other.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }
    }
}