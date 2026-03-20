using UnityEngine;

public class PlayerFootstepSFX : MonoBehaviour
{   
    private PlayerController playerController;
    public AudioSource audioSource;
    public AudioClip[] grassWalkClips;
    public AudioClip[] grassRunClips;
    public AudioClip[] concreteWalkClips;
    public AudioClip[] concreteRunClips;
    AudioClip[] walkClips;
    AudioClip[] runClips;

    void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
        audioSource = GetComponent<AudioSource>();

        walkClips = grassWalkClips;
        runClips = grassRunClips;
    }

    void Update()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1f))
        {
            if (hit.collider.gameObject.layer == 10) // grass
            {
                walkClips = grassWalkClips;
                runClips = grassRunClips;
            }else if (hit.collider.gameObject.layer == 11) // concrete
            {
                walkClips = concreteWalkClips;
                runClips = concreteRunClips;
            }
        }
    }

    void PlayFootstep()
    {
        if (walkClips.Length == 0 || runClips.Length == 0)
            {
                return;
            }

        if (playerController.isMoving == true)
            {
                AudioClip[] clips = playerController.isRunning ? runClips : walkClips;

                audioSource.pitch = Random.Range(0.9f, 1.1f);
                float volume = Random.Range(0.2f, 0.3f);
                AudioClip clip = clips[Random.Range(0, walkClips.Length)];
                audioSource.PlayOneShot(clip, volume);

                NoiseSystem.MakeNoise(transform.position + Vector3.forward, 2f);
            }
    }
}