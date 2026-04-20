using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DangerVision : MonoBehaviour
{
    [SerializeField] AudioClip soundEffect;
    [SerializeField] AudioClip blinkingEffect;
    public StatueMonsterAi statueMonsterAi;
    public Volume postProcessingVolume;
    AudioSource audioSource;

    [SerializeField] float cooldownTime = 15f;
    float cooldownTimer = 0f;
    [SerializeField] float effectTime = 2.5f;
    float effectTimer = 0f;
    [SerializeField] float vignetteSpeed = 1f;
    [SerializeField] float chromaticAberrationSpeed = 1f;

    [SerializeField] float vignetteIntensity = 0.6f;
    [SerializeField] float chromaticAberrationIntensity = 0.6f;

    private Vignette vignette;
    private ChromaticAberration chromaticAberration;

    bool isEffectActive = false;
    bool hasTriggeredThisLook = false;

    private void Start() 
    {
        audioSource = GetComponent<AudioSource>();
        postProcessingVolume.profile.TryGet(out vignette);
        postProcessingVolume.profile.TryGet(out chromaticAberration);
        cooldownTimer = 0f;
    }

    void Update()
    {
        normalJumpscare();
    }

    void ActivateEffect()
    {
        vignette.intensity.value = Mathf.MoveTowards(vignette.intensity.value, vignetteIntensity, Time.deltaTime * vignetteSpeed);
        chromaticAberration.intensity.value = Mathf.MoveTowards(chromaticAberration.intensity.value, chromaticAberrationIntensity, Time.deltaTime * chromaticAberrationSpeed);
    }

    void DeactivateEffect()
    {
        vignette.intensity.value = Mathf.MoveTowards(vignette.intensity.value, 0.18f, Time.deltaTime * vignetteSpeed);
        chromaticAberration.intensity.value = Mathf.MoveTowards(chromaticAberration.intensity.value, 0f, Time.deltaTime * chromaticAberrationSpeed);
    }

    void normalJumpscare()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (!statueMonsterAi.isSeen)
        {
            if (hasTriggeredThisLook)
            {
                cooldownTimer = cooldownTime;
                hasTriggeredThisLook = false;
            }
        }

        if (statueMonsterAi.isSeen && cooldownTimer > 0f && !hasTriggeredThisLook)
        {
            cooldownTimer = cooldownTime;
        }
        
        bool canTrigger = statueMonsterAi.isSeen 
                       && statueMonsterAi.gameObject.activeInHierarchy 
                       && cooldownTimer <= 0f
                       && !hasTriggeredThisLook;

        if (canTrigger && !isEffectActive)
        {
            hasTriggeredThisLook = true;
            effectTimer = effectTime;
            isEffectActive = true;
            audioSource.PlayOneShot(soundEffect);
        }

        if (isEffectActive)
        {
            if (effectTimer > 0f)
            {
                ActivateEffect();
                effectTimer -= Time.deltaTime;
            }
            else
            {
                DeactivateEffect();

                if (vignette.intensity.value <= 0.19f && chromaticAberration.intensity.value <= 0.01f)
                {
                    vignette.intensity.value = 0.18f;
                    chromaticAberration.intensity.value = 0f;
                    isEffectActive = false;
                }
            }
        }
    }

    public void TriggerBlinkingEffect()
    {
        if (isEffectActive) return;

        isEffectActive = true;
        effectTimer = effectTime;
        audioSource.PlayOneShot(blinkingEffect, 2f);
    }
}
