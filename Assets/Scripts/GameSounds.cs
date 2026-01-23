using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;

public class GameSounds : MonoBehaviour
{
    PlayerController playerController;

    Rigidbody rb;
    AudioManager s;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerController = GetComponent<PlayerController>();
        s = FindFirstObjectByType<AudioManager>();
    }

    void Update()
    {
        
    }
}
        
            
    
