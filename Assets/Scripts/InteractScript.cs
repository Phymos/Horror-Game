using UnityEngine;
using UnityEngine.InputSystem;

public class InteractScript : MonoBehaviour
{
    Camera Cam;
    
    LayerMask interactableDoorLayer;
    LayerMask interactableKeyLayer;
    LayerMask interactableNextLevelLayer;

    public LevelLoader levelLoader;
    public int nextLevelNo;

    void Awake()
    {
        Cam = Camera.main;
        interactableDoorLayer = LayerMask.GetMask("DoorLayer");
        interactableKeyLayer = LayerMask.GetMask("KeyLayer");
        interactableNextLevelLayer = LayerMask.GetMask("NextLevelLayer");
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out RaycastHit hit, 3f, interactableDoorLayer))
            {
                DoorInteract doorOpenCloseScript = hit.collider.GetComponent<DoorInteract>();
                doorOpenCloseScript.doorOpenClose();
            }
            else if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out RaycastHit hit2, 3f, interactableKeyLayer))
            {
                pickUpKey pickUpKeyScript = hit2.collider.GetComponent<pickUpKey>();
                pickUpKeyScript.keyPickUp();
            }
            else if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out RaycastHit hit3, 3f, interactableNextLevelLayer))
            {
                levelLoader.LoadLevel(nextLevelNo);
            }
            else
            {
                return;
            }
        }
    }
}
