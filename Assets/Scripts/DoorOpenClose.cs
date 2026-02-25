using UnityEngine;
using UnityEngine.InputSystem;

public class DoorOpenClose : MonoBehaviour
{
    Animator anim;
    bool isOpen = false;
    
    public Collider doorCollider;

    Camera cam;

    void Start()
    {
        anim = GetComponent<Animator>();
        cam = Camera.main;
    }

    void Update()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hitInfo, 3f))
        {
            if (hitInfo.collider == doorCollider)
            {
                if (Keyboard.current.eKey.wasPressedThisFrame)
                {
                    isOpen = !isOpen;
                    anim.SetBool("isOpen", isOpen);
                }
            }
        }
    }
}