using UnityEngine;

public class MonsterFootsteps : MonoBehaviour
{
    private MonsterAi monsterAi;
    public AudioSource audioSource;
    public AudioClip[] grassWalkClips;
    public AudioClip[] grassChaseClips;
    public AudioClip[] concreteWalkClips;
    public AudioClip[] concreteChaseClips;
    AudioClip[] walkClips;
    AudioClip[] chaseClips;

    void Awake()
    {
        monsterAi = GetComponentInParent<MonsterAi>();
        audioSource = GetComponentInParent<AudioSource>();

        walkClips = grassWalkClips;
        chaseClips = grassChaseClips;
    }

    void Update()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1f))
        {
            if (hit.collider.gameObject.layer == 10) // grass
            {
                walkClips = grassWalkClips;
                chaseClips = grassChaseClips;
            }else if (hit.collider.gameObject.layer == 11) // concrete
            {
                walkClips = concreteWalkClips;
                chaseClips = concreteChaseClips;
            }
        }
    }

    void PlayMonsterFootstep()
    {
        if (walkClips.Length == 0 || chaseClips.Length == 0)
            {
                return;
            }
        AudioClip[] clips = monsterAi.isChasing ? chaseClips : walkClips;

        audioSource.pitch = Random.Range(0.9f, 1.1f);
        float volume = Random.Range(0.2f, 0.3f);
        AudioClip clip = clips[Random.Range(0, walkClips.Length)];
        audioSource.PlayOneShot(clip, volume);
    }
}
