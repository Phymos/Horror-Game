using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class NoiseEmitter : MonoBehaviour
{
    [SerializeField] float intensity = 2f;
    [SerializeField] Object NoiseEmitterObject;
    [SerializeField] AudioClip noiseClip;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NoiseSystem.MakeNoise(transform.position, intensity);

            audioSource.PlayOneShot(noiseClip);

            Rigidbody[] rbs = GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rb in rbs)
            {
                rb.isKinematic = false;
            }

            Destroy(GetComponent<Collider>());
        }
    }
}
