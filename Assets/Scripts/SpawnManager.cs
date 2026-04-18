using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Transform basementExitSpawn;
    public GameObject player;

    void Start()
    {
        if (GameManager.Instance.lastExitPoint == "basement")
        {
            player.transform.position = basementExitSpawn.position;
        }
    }
}
