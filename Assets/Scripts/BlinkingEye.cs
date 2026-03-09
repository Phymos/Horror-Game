using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingEye : MonoBehaviour
{
    [SerializeField] float blinkMeter = 10f;
    float blinkTimer;
    [SerializeField] float drainSpeed = 1f;
    public Image blinkScreen;
    public bool isBlinking = false;

    [SerializeField] float fadeInSpeed = 25f;
    [SerializeField] float fadeOutSpeed = 15f;

    void Start()
    {
        blinkTimer = blinkMeter;
    }

    void Update()
    {
        blinkTimer -= Time.deltaTime * drainSpeed;

        if (blinkTimer <= 0)
        {
            blinkTimer = blinkMeter;
            StartCoroutine(Blink());
        }
    }

    IEnumerator Blink()
    {
        Color c = blinkScreen.color;
        isBlinking = true;

        while (c.a < 0.99f)
        {
            c.a = Mathf.Lerp(c.a, 1f, Time.deltaTime * fadeInSpeed);
            blinkScreen.color = c;
            yield return null;
        }
        c.a = 1f;
        blinkScreen.color = c;
        
        yield return new WaitForSeconds(0.15f);
        
        while (c.a > 0.01f)
        {
            c.a = Mathf.Lerp(c.a, 0f, Time.deltaTime * fadeOutSpeed);
            blinkScreen.color = c;
            yield return null;
        }
        
        c.a = 0f;
        blinkScreen.color = c;
        isBlinking = false;
    }
}
