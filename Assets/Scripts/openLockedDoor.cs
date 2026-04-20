using UnityEngine;

public class OpenLockedDoor : MonoBehaviour
{
    public string requiredKeyId;
    public AudioClip lockedSound;
    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void TryOpen()
    {
        if (GameManager.Instance.keys.Contains(requiredKeyId))
        {
            DoorInteract doorInteract = GetComponent<DoorInteract>();
            doorInteract.DoorOpenClose();
        }
        else
        {
            audioSource.PlayOneShot(lockedSound);
        }
    }
}
