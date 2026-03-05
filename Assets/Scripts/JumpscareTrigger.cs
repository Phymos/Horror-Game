using UnityEngine;

public class JumpscareTrigger : MonoBehaviour
{
    public Transform cameraPivot;
    AudioSource audioSource;
    //public AudioClip scaryClip;

    bool triggered = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (triggered)
        {
            cameraPivot.localRotation = Quaternion.RotateTowards(cameraPivot.localRotation, Quaternion.Euler(20f, 0f, 85f), 2000f * Time.deltaTime);

            cameraPivot.localRotation *= Quaternion.Euler(0, 0, Random.Range(-1f, 1f));

            //bir fotoğraf olacak siyah, yavaşça siyaha dönecek.
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