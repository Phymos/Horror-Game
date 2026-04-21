using UnityEngine;

public class StatueScrapeSound : MonoBehaviour
{
    private StatueMonsterAi statueAi;
    AudioSource audioSource;
    public AudioClip scrapeClip;
    public float intensity;

    void Awake()
    {
        statueAi = GetComponent<StatueMonsterAi>();
        audioSource = GetComponent<AudioSource>();
        
        audioSource.clip = scrapeClip;
        audioSource.loop = true;
    }

    void Update()
    {
        if (!statueAi.enabled) return;

        bool isMoving = statueAi.agent.velocity.magnitude > 0.1f;

        if (isMoving)
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
            
            NoiseSystem.MakeNoise(transform.position, intensity);
        }
        else if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
