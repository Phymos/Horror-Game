using Unity.VisualScripting;
using UnityEngine;

public class NoiseEmitter : MonoBehaviour
{
    [SerializeField] float intensity = 2f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NoiseSystem.MakeNoise(transform.position, intensity);
        }
    }
}
