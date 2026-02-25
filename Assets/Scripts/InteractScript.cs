using UnityEngine;
using UnityEngine.InputSystem;

public class InteractScript : MonoBehaviour
{
    Camera Cam;
    
    LayerMask interactableDoorLayer = LayerMask.GetMask("DoorLayer");
    LayerMask interactableKeyLayer = LayerMask.GetMask("KeyLayer");

    pickUpKey pickUpKeyScript;
    DoorInteract doorOpenCloseScript;

    void Awake()
    {
        Cam = Camera.main;
    }

    void Update()
    {
        
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out RaycastHit hit, 3f, interactableDoorLayer))
            {
                doorOpenCloseScript.doorOpenClose();
            }
            else if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out RaycastHit hit2, 3f, interactableKeyLayer))
            {
                pickUpKeyScript.keyPickUp();
            }
        }
    }
}
