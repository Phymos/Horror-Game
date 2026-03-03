using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    public AudioSource sfxAudioSource;
    public AudioClip chainClip;
    public float velocityThreshold = 1f;
    
    private Rigidbody rb;
    private bool isPlaying = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (rb.linearVelocity.magnitude > velocityThreshold && !isPlaying)
        {
            sfxAudioSource.pitch = Random.Range(0.8f, 1.2f);
            sfxAudioSource.PlayOneShot(chainClip);
            isPlaying = true;
        }
        
        if (rb.linearVelocity.magnitude <= velocityThreshold)
        {
            isPlaying = false;
        }
    }
}
