using UnityEngine;

public class ActivationTrigger : MonoBehaviour
{
    public GameObject objectToActivate;

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            objectToActivate.SetActive(true);
        }
    }
}