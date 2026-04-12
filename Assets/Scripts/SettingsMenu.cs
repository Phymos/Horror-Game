using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Volume postProcessingVolume;
    private ColorAdjustments colorAdjustments;
    public PlayerController playerController;

    void Start()
    {
        postProcessingVolume.profile.TryGet(out colorAdjustments);

        float savedBrightness = PlayerPrefs.GetFloat("Brightness", 0f);
        SetBrightness(savedBrightness);

        float savedVolume = PlayerPrefs.GetFloat("Volume", 0f);
        SetVolume(savedVolume);

        float savedSensivity = PlayerPrefs.GetFloat("Sensivity", 1f);
        SetSensivity(savedSensivity);
    }

    public void SetBrightness(float brightness)
    {
        colorAdjustments.postExposure.value = brightness;
        PlayerPrefs.SetFloat("Brightness", brightness);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);

        PlayerPrefs.SetFloat("Volume", volume);
    }

    public void SetSensivity(float sensivity)
    {
        PlayerPrefs.SetFloat("Sensivity", sensivity);
        playerController.sensivity = sensivity;
    }
}
