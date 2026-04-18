using UnityEngine;
using UnityEngine.InputSystem;

public class OpenLockedDoor : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;

    public Collider doorCollider;


    void Update()
    {
        ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
    }

    void checkAction()
    {
        if(GameManager.Instance.hasKey && Physics.Raycast(ray, out hit, 100))
        {
            if(hit.collider == doorCollider)
            {
                if (Keyboard.current.eKey.wasPressedThisFrame)
                {
                    Destroy(doorCollider.gameObject);
                }
            }
        }
    }
}
