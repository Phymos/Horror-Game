using UnityEngine;
using UnityEngine.InputSystem;

public class EndingTrigger : MonoBehaviour
{
    public GameObject endingUI;

    public Animator transition;

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            endingUI.SetActive(true);

            other.transform.root.GetComponent<PlayerInput>().enabled = false;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
