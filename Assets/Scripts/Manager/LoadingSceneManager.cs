using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class LoadingSceneManager : MonoBehaviour, IManager
{
    [Header("Loading UI")]
    public GameObject loadingScreen;  // 로딩 화면 오브젝트
    public Slider progressSlider; // 로딩 진행 바
    public Text progressText; // 퍼센티지 텍스트

    private void Start()
    {
        GameManager.LoadingScene = this;

        // 처음에는 로딩 화면 비활성화
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
        }
    }

    // 씬을 비동기로 로드하고 로딩 화면을 표시
    public void LoadScene(int sceneIndex)
    {
        // 로딩 화면 활성화
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }

        // 비동기로 씬을 로드하는 코루틴 시작
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            UpdateProgress(operation.progress);
            yield return null;
        }

        // GenericUnit의 초기화를 기다림
        // yield return new WaitUntil(() => FindObjectOfType<GenericUnit>().InitializationComplete);

        // 로딩 화면 비활성화
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
        }
    }

    void UpdateProgress(float progress)
    {
        float displayProgress = Mathf.Clamp01(progress / 0.9f);
        if (progressSlider != null)
            progressSlider.value = displayProgress;
        if (progressText != null)
            progressText.text = Mathf.RoundToInt(displayProgress * 100f) + "%";
    }

    public void Clear()
    {

    }
}
