using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class pickUpKey : MonoBehaviour
{
    public static bool isKeyPickedUp = false;
    public Collider keyCollider;
    bool raycastCheck = false;

    public List<GameObject> Monsters;
    public StatueMonsterAi statueMonsterAi;

    Ray ray;
    RaycastHit hit;

    void Update()
    {
        ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        checkPickUp();
    }

    void checkPickUp()
    {
        if(Physics.Raycast(ray, out hit, 100))
        {
            if(hit.collider == keyCollider)
            {
                if (Keyboard.current.eKey.wasPressedThisFrame)
                {
                    isKeyPickedUp = true;
                    Destroy(keyCollider.gameObject);
                    foreach (GameObject monster in Monsters)
                    {
                        statueMonsterAi = monster.GetComponent<StatueMonsterAi>();
                        statueMonsterAi.enabled = true;
                    }
                }
            }
        }
    }
}