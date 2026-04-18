using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickUpKey : MonoBehaviour
{
    public Collider keyCollider;

    public List<GameObject> Monsters;
    public StatueMonsterAi statueMonsterAi;

    public GameObject objectToActivate;

    public void KeyPickUp()
    {
        GameManager.Instance.hasKey = true;
        
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