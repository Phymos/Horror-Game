using UnityEngine;

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
    }
}