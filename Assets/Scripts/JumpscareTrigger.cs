using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class JumpscareTrigger : MonoBehaviour
{
    [SerializeField] float cameraRotateSpeed = 2000f;
    [SerializeField] float cameraFallSpeed = 1f;
    [SerializeField] float cameraShakeIntensity = 1f;
    [SerializeField] float vignetteSpeed = 1f;
    [SerializeField] float blackScreenSpeed = 1f;


    public Transform cameraPivot;
    AudioSource audioSource;
    //public AudioClip scaryClip;
    
    public Volume volume;
    public Image blackScreen;
    Vignette vignette;

    bool triggered = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        volume.profile.TryGet<Vignette>(out vignette);
    }

    void Update()
    {
        if (triggered)
        {
            cameraPivot.localRotation = Quaternion.RotateTowards(cameraPivot.localRotation, Quaternion.Euler(20f, 0f, 85f), cameraRotateSpeed * Time.deltaTime);

            cameraPivot.localRotation *= Quaternion.Euler(0, 0, Random.Range(-cameraShakeIntensity, cameraShakeIntensity));

            //oyuncunun kafası(kamerası) aşağı düşecek  ///belli bir sebepten ötürü 0.41 olmuyor ancak sonuç yeterli bence.
            cameraPivot.localPosition = Vector3.MoveTowards(cameraPivot.localPosition, new Vector3(0f, 0.41f, 0f), Time.deltaTime * cameraFallSpeed);

            //bir fotoğraf olacak siyah, yavaşça siyaha dönecek.
            vignette.intensity.value = Mathf.MoveTowards(vignette.intensity.value, 0.9f, Time.deltaTime * vignetteSpeed);
            Color c = blackScreen.color;
            c.a = Mathf.MoveTowards(blackScreen.color.a, 0.8f, Time.deltaTime * blackScreenSpeed);
            blackScreen.color = c;
        }   

    }

    void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        other.GetComponent<PlayerController>().enabled = false;
        other.GetComponentInChildren<Animator>().enabled = false;

        triggered = true;

        //audioSource.PlayOneShot(scaryClip);
    }
}