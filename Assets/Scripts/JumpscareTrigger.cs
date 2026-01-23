using Unity.Cinemachine;
using UnityEngine;

public class JumpscareTrigger : MonoBehaviour
{
    public Object player;
    public CinemachineCamera playerCam;
    public CinemachineCamera jumpscareCam;
    AudioSource audioSource;
    public AudioClip scaryClip;
    Vector3 triggerPos;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        triggerPos = transform.localPosition;
    }

    void OnTriggerEnter(Collider player)
    {
        audioSource.PlayOneShot(scaryClip);
        jumpscareCam.Priority = 2;
    }
}