using UnityEngine;

public class DoorInteract : MonoBehaviour
{
    public AudioClip[] doorSounds;
    AudioSource audioSource;
    Animator doorAnim;
    public bool isOpen;
    public float noiseIntensity;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        doorAnim = GetComponent<Animator>();
    }

    public void doorOpenClose()
    {
        isOpen = !isOpen;
        doorAnim.SetBool("isOpen", isOpen);
        NoiseSystem.MakeNoise(GameObject.FindWithTag("Player").transform.position, noiseIntensity);

        if (isOpen)
        {
            audioSource.PlayOneShot(doorSounds[0]);
        }
        else
        {
            audioSource.PlayOneShot(doorSounds[1]);
        }
    }
}
