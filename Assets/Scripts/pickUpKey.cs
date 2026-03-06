using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class pickUpKey : MonoBehaviour
{
    public static bool isKeyPickedUp = false;
    public Collider keyCollider;

    public List<GameObject> Monsters;
    public StatueMonsterAi statueMonsterAi;

    public GameObject objectToActivate;

    public void keyPickUp()
    {
        isKeyPickedUp = true;
        Destroy(keyCollider.gameObject);
        foreach (GameObject monster in Monsters)
        {
            statueMonsterAi = monster.GetComponent<StatueMonsterAi>();
            statueMonsterAi.enabled = true;
        }

        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
        }
    }
}