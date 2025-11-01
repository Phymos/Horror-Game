using UnityEngine;
using UnityEngine.InputSystem;

public class DoorOpenClose : MonoBehaviour
{
    Animator anim;
    bool isOpen = false;
    public Transform player;
    public Collider doorCollider;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Physics.Raycast(player.position, player.forward, out RaycastHit hitInfo, 4f))
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