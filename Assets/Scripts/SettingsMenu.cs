using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Volume postProcessingVolume;
    private ColorAdjustments colorAdjustments;

    void Start()
    {
        postProcessingVolume.profile.TryGet(out colorAdjustments);

        float savedBrightness = PlayerPrefs.GetFloat("Brightness", 0f);
        SetBrightness(savedBrightness);

        float savedVolume = PlayerPrefs.GetFloat("Volume", 0f);
        SetVolume(savedVolume);
    }

    public void SetBrightness(float brightness)
    {
        colorAdjustments.postExposure.value = brightness;
        PlayerPrefs.SetFloat("Brightness", brightness);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);

        float savedVolume = PlayerPrefs.GetFloat("Volume", 0f);
        PlayerPrefs.SetFloat("Volume", volume);
    }
}
