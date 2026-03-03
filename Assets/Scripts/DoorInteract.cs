using UnityEngine;

public class DoorInteract : MonoBehaviour
{
    public AudioClip[] doorSounds;
    AudioSource audioSource;
    Animator doorAnim;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        doorAnim = GetComponent<Animator>();
    }

    public void doorOpenClose()
    {
        bool isOpen = doorAnim.GetBool("isOpen");  //starts as false (its closed)
        doorAnim.SetBool("isOpen", !isOpen);

        Debug.Log("isOpen was: " + isOpen + " | clip: " + (!isOpen ? doorSounds[0]?.name : doorSounds[1]?.name));

        if (!isOpen)
        {
            audioSource.PlayOneShot(doorSounds[0]);
        }
        else
        {
            audioSource.PlayOneShot(doorSounds[1]);
        }
    }
}
