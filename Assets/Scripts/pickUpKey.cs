using UnityEngine;
using UnityEngine.InputSystem;

public class pickUpKey : MonoBehaviour
{
    bool isKeyPickedUp = false;
    public Collider keyCollider;
    bool raycastCheck = false;

    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    RaycastHit hit;

    void Update()
    {
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
                }
            }
        }
    }
}
