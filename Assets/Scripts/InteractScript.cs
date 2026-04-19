using UnityEngine;
using UnityEngine.InputSystem;

public class InteractScript : MonoBehaviour
{
    Camera Cam;
    
    LayerMask interactableDoorLayer;
    LayerMask interactableKeyLayer;
    LayerMask interactableNextLevelLayer;
    LayerMask interactableLockedDoorLayer;

    public LevelLoader levelLoader;
    public int nextLevelNo;

    void Awake()
    {
        Cam = Camera.main;
        interactableDoorLayer = LayerMask.GetMask("DoorLayer");
        interactableKeyLayer = LayerMask.GetMask("KeyLayer");
        interactableNextLevelLayer = LayerMask.GetMask("NextLevelLayer");
        interactableLockedDoorLayer = LayerMask.GetMask("LockedDoorLayer");
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out RaycastHit hit, 3f, interactableDoorLayer))
            {
                DoorInteract doorOpenCloseScript = hit.collider.GetComponent<DoorInteract>();
                doorOpenCloseScript.DoorOpenClose();
            }
            else if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out RaycastHit hit2, 3f, interactableKeyLayer))
            {
                PickUpKey pickUpKeyScript = hit2.collider.GetComponent<PickUpKey>();
                pickUpKeyScript.KeyPickUp();
            }
            else if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out RaycastHit hit3, 3f, interactableNextLevelLayer))
            {
                string exitPoint = nextLevelNo == 1 ? "basement" : "";
                levelLoader.LoadLevel(nextLevelNo, exitPoint);
            }
            else if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out RaycastHit hit4, 3f, interactableLockedDoorLayer))
            {
                OpenLockedDoor openLockedDoor = hit4.collider.GetComponent<OpenLockedDoor>();
                openLockedDoor.TryOpen();
            }
        }
    }
}
