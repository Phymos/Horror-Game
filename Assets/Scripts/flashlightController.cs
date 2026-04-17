using UnityEngine;
using UnityEngine.InputSystem;

public class flashlightController : MonoBehaviour
{
    [SerializeField] GameObject flashlightObj;
    [SerializeField] float flashOnFogDensity;
    [SerializeField] float flashOffFogDensity;
    [SerializeField] AudioClip flashlightSound;
    bool flashOnOff = false;

    public float intensity;

    AudioSource audioSource;
    
    void Awake()
    {
        flashOnOff = flashlightObj.activeSelf;
        audioSource = GetComponent<AudioSource>();
    }

    public void Flashlight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            flashOnOff = !flashOnOff;
            flashlightObj.SetActive(flashOnOff);
            audioSource.PlayOneShot(flashlightSound);

            NoiseSystem.MakeNoise(transform.position, intensity);

            if (flashOnOff)
            {
                RenderSettings.fogDensity = flashOnFogDensity;
            }
            else
            {
                RenderSettings.fogDensity = flashOffFogDensity;
            }
        }
    }
}
