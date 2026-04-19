using System.Data.Common;
using UnityEngine;
using UnityEngine.InputSystem;

public class OpenLockedDoor : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;

    public Collider doorCollider;

    public string requiredKeyId;

    void Update()
    {
        CheckAction();
    }

    void CheckAction()
    {
        ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        if(GameManager.Instance.keys.Contains(requiredKeyId) && Physics.Raycast(ray, out hit, 100))
        {
            if(hit.collider == doorCollider)
            {
                if (Keyboard.current.eKey.wasPressedThisFrame)
                {
                    DoorInteract doorOpenCloseScript = hit.collider.GetComponent<DoorInteract>();
                    doorOpenCloseScript.DoorOpenClose();
                }
            }
        }
    }
}
