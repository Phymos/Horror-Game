using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OptionsMenu : MonoBehaviour
{
    [Header("Audio")]
    public AudioMixer audioMixer;
    public TMPro.TextMeshProUGUI volumeText;
    public UnityEngine.UI.Slider volumeSlider;

    [Header("Brightness")]
    public Volume postProcessingVolume;
    private ColorAdjustments colorAdjustments;
    public TMPro.TextMeshProUGUI brightnessText;
    public UnityEngine.UI.Slider brightnessSlider;

    [Header("Sensivity")]
    public UnityEngine.UI.Slider sensivitySlider;
    public TMPro.TextMeshProUGUI sensivityText;

    void Start()
    {
        postProcessingVolume.profile.TryGet(out colorAdjustments);

        float savedBrightness = PlayerPrefs.GetFloat("Brightness", 0f);
        SetBrightness(savedBrightness);
        brightnessSlider.value = savedBrightness;

        float savedVolume = PlayerPrefs.GetFloat("Volume", 0f);
        SetVolume(savedVolume);
        volumeSlider.value = savedVolume;

        float savedSensivity = PlayerPrefs.GetFloat("Sensivity", 1f);
        SetSensivity(savedSensivity);
        sensivitySlider.value = savedSensivity;
    }

    public void SetBrightness(float brightness)
    {
        colorAdjustments.postExposure.value = brightness;
        PlayerPrefs.SetFloat("Brightness", brightness);

        brightnessText.text = Mathf.RoundToInt(brightness * 50) + "%";
    }

    public void SetVolume(float volume)
    {
        if (volume <= 0.01f)
        {
            audioMixer.SetFloat("Volume", -80f);
        }
        else
        {
            audioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
        }

        PlayerPrefs.SetFloat("Volume", volume);

        volumeText.text = Mathf.RoundToInt(volume * 100) + "%";
    }

    public void SetSensivity(float sensivity)
    {
        PlayerPrefs.SetFloat("Sensivity", sensivity);

        sensivityText.text = Mathf.RoundToInt(sensivity) + "%";
    }
}
