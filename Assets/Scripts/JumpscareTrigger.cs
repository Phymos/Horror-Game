using Unity.Cinemachine;
using UnityEngine;

public class JumpscareTrigger : MonoBehaviour
{
    public CinemachineCamera playerCam;
    public CinemachineCamera jumpscareCam;
    AudioSource audioSource;
    public AudioClip scaryClip;

    bool triggered = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;

        audioSource.PlayOneShot(scaryClip);
        jumpscareCam.Priority = 2;
    }
}