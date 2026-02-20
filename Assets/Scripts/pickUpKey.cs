using UnityEngine;
using UnityEngine.InputSystem;

public class pickUpKey : MonoBehaviour
{
    public static bool isKeyPickedUp = false;
    public Collider keyCollider;
    bool raycastCheck = false;

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
                }
            }
        }
    }
}