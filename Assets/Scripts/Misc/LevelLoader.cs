using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;
    public TextMeshProUGUI progressText;

    public void LoadLevel(int sceneIndex, string exitPoint = "")
    {
        GameManager.Instance.lastExitPoint = exitPoint;
        StartCoroutine(LoadAsync(sceneIndex));
    }

    public void LoadLevel(int sceneIndex)
    {
        LoadLevel(sceneIndex, "");
    }


    IEnumerator LoadAsync (int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            progressText.text = (progress * 100f).ToString("F0") + "%";

            yield return null;
        }
    }
}