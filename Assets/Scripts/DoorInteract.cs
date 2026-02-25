using UnityEngine;

public class DoorInteract : MonoBehaviour
{
    public void doorOpenClose()
    {
        Animator doorAnim = GetComponent<Animator>();
        bool isOpen = doorAnim.GetBool("isOpen");
        doorAnim.SetBool("isOpen", !isOpen);
    }
}
