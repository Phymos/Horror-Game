using UnityEngine;

public class ActivationTrigger : MonoBehaviour
{
    public GameObject objectToActivate;
    public AudioClip clip;
    public AudioSource audioSource;

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            objectToActivate.SetActive(true);
            if (clip != null && audioSource != null)
            {
                audioSource.PlayOneShot(clip);
            }

            GetComponent<Collider>().enabled = false;
        }
    }
}