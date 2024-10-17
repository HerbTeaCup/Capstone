using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class LoadingSceneManager : MonoBehaviour, IManager
{
    [Header("Loading UI")]
    public GameObject loadingScreen;  // �ε� ȭ�� ������Ʈ
    public Slider progressSlider; // �ε� ���� ��
    public Text progressText; // �ۼ�Ƽ�� �ؽ�Ʈ

    private void Start()
    {
        GameManager.LoadingScene = this;

        // ó������ �ε� ȭ�� ��Ȱ��ȭ
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
        }
    }

    // ���� �񵿱�� �ε��ϰ� �ε� ȭ���� ǥ��
    public void LoadScene(int sceneIndex)
    {
        // �ε� ȭ�� Ȱ��ȭ
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }

        // �񵿱�� ���� �ε��ϴ� �ڷ�ƾ ����
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

        // GenericUnit�� �ʱ�ȭ�� ��ٸ�
        // yield return new WaitUntil(() => FindObjectOfType<GenericUnit>().InitializationComplete);

        // �ε� ȭ�� ��Ȱ��ȭ
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
