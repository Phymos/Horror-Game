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

    public Image fill;

    float noiseLevel;

    void Start()
    {
        noiseLevel = 0;
    }

    // Update is called once per frame
    void Update()
    {
        HandleSlider();
    }

    void HandleSlider()
    {
        if (noiseSlider.value > 0)
        {
            noiseSlider.value -= Time.deltaTime * sliderSpeed;
            fill.color = gradient.Evaluate(noiseSlider.value);
        }
    }

    void HandleNoiseLevel(Vector3 position, float intensity)
    {
        noiseLevel = intensity / Vector3.Distance(transform.position ,position);

        noiseSlider.value = noiseLevel/maxValue;
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
