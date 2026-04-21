using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip ambianceClip;
    public AudioSource audioSource;

    void Start()
    {
        audioSource.PlayOneShot(ambianceClip);
    }
}
