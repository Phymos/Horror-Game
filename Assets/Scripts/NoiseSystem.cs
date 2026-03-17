using System;
using UnityEngine;

public static class NoiseSystem
{
    public static event Action<Vector3, float> OnNoiseMade;

    public static void MakeNoise(Vector3 position, float intensity)
    {
        OnNoiseMade?.Invoke(position, intensity);
    }
}