using UnityEngine;

public class HeadbobSystem : MonoBehaviour
{
    [Range(0.001f, 0.1f)]
    public float Amount = 0.01f;

    [Range(1f, 30f)]
    public float Frequency = 12f;

    [Range(5f, 20f)]
    public float Smooth = 10f;

    Vector3 startPos;
    float timer;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float inputMagnitude = new Vector3(
            Input.GetAxis("Horizontal"),
            0,
            Input.GetAxis("Vertical")
        ).magnitude;

        if (inputMagnitude > 0.1f)
        {
            StartHeadBob();
        }
        else
        {
            StopHeadBob();
        }
    }

    void StartHeadBob()
    {
        timer += Time.deltaTime * Frequency;

        Vector3 bobPos = Vector3.zero;
        bobPos.y = Mathf.Sin(timer) * Amount;
        bobPos.x = Mathf.Cos(timer / 2f) * Amount * 0.6f;

        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            startPos + bobPos,
            Smooth * Time.deltaTime
        );
    }

    void StopHeadBob()
    {
        timer = 0;
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            startPos,
            Smooth * Time.deltaTime
        );
    }
}