using UnityEngine;
using UnityEngine.InputSystem;

public class openLockedDoor : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;

    bool isKeyPickedUp = false;

    public Collider doorCollider;


    void Update()
    {
        ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        isKeyPickedUp = pickUpKey.isKeyPickedUp;
    }

    void checkAction()
    {
        if(Physics.Raycast(ray, out hit, 100))
        {
            if(hit.collider == doorCollider)
            {
                if (Keyboard.current.eKey.wasPressedThisFrame)
                {
                    isKeyPickedUp = true;
                    Destroy(doorCollider.gameObject);
                }
            }
        }
    }
}
