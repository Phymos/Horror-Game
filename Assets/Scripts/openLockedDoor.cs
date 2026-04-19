using System.Data.Common;
using UnityEngine;
using UnityEngine.InputSystem;

public class OpenLockedDoor : MonoBehaviour
{
    public string requiredKeyId;

    public void TryOpen()
    {
        if (GameManager.Instance.keys.Contains(requiredKeyId))
        {
            DoorInteract doorInteract = GetComponent<DoorInteract>();
            doorInteract.DoorOpenClose();
        }
    }
}
