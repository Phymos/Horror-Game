using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NoiseSlider : MonoBehaviour
{
    [SerializeField] Slider noiseSlider;
    [SerializeField] float maxValue = 10f;
    [SerializeField] float sliderSpeed = 3f;
    [SerializeField] Gradient gradient;
    [SerializeField] float cooldownTimer;
    float cooldownTime;
 
    public Image fill;

    float noiseLevel;
    float targetValue;

    void Start()
    {
        noiseLevel = 0;
    }

    void Update()
    {
        HandleSlider();
    }

    void HandleSlider()
    {
        if (targetValue > noiseSlider.value)
        {
            noiseSlider.value = targetValue;
        }
        else
        {
            noiseSlider.value = Mathf.Lerp(noiseSlider.value, targetValue, Time.deltaTime * sliderSpeed);
            fill.color = gradient.Evaluate(noiseSlider.value);
        }

        if (cooldownTime > 0)
        {
            cooldownTime -= Time.deltaTime;
        }
        else
        {
            targetValue = 0;
        }
    }

    void HandleNoiseLevel(Vector3 position, float intensity)
    {
        float dist = Vector3.Distance(transform.position, position);
        dist = Mathf.Max(dist, 0.001f);

        noiseLevel = intensity / dist;

        targetValue = Mathf.Clamp01(noiseLevel / maxValue);

        cooldownTime = cooldownTimer;
    }

    void OnEnable()
    {
        NoiseSystem.OnNoiseMade += HandleNoiseLevel;
    }

    void OnDisable()
    {
        NoiseSystem.OnNoiseMade -= HandleNoiseLevel;
    }
}
