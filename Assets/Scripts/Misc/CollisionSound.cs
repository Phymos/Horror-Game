using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    public AudioSource sfxAudioSource;
    public AudioClip chainClip;
    public float velocityThreshold = 1f;
    
    private Rigidbody rb;
    private bool isPlaying = false;

    public float noiseIntensity = 8f;

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
            NoiseSystem.MakeNoise(transform.position, noiseIntensity);
        }
        
        if (rb.linearVelocity.magnitude <= velocityThreshold)
        {
            isPlaying = false;
        }
    }
}
