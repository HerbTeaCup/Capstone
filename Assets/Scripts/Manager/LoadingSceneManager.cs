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

    private static LoadingSceneManager _instance;

    private void Start()
    {
        // �̱��� ���� �������� �ߺ� ���� ����
        if (_instance == null)
        {
            _instance = this;
            GameManager.LoadingScene = this;
            DontDestroyOnLoad(gameObject);
            EnsureUIComponents();
            Debug.Log("LoadingSceneManager �ν��Ͻ��� �����Ǿ����ϴ�.");
        }
        else
        {
            Destroy(gameObject);  // �ߺ��� �ν��Ͻ��� ����
            Debug.LogWarning("LoadingSceneManager �ߺ� ������ �����Ǿ� �ı��Ǿ����ϴ�.");
        }

        // ó������ �ε� ȭ�� ��Ȱ��ȭ
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
        }
    }

    // ���� �񵿱�� �ε��ϰ� �ε� ȭ���� ǥ��
    public void LoadScene(int sceneIndex)
    {
        if (SceneManager.GetActiveScene().buildIndex == sceneIndex)
        {
            Debug.LogWarning("�̹� Ȱ��ȭ�� ���Դϴ�.");
            return;
        }

        EnsureUIComponents();

        // �ε� ȭ�� Ȱ��ȭ
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
            Debug.Log("Loading Screen Ȱ��ȭ");
        }
        else
        {
            Debug.LogError("Loading Screen�� �������� �ʽ��ϴ�.");
        }

        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    private void EnsureUIComponents()
    {
        // �ε� ȭ���� ã��, ������ Resources���� �ε�
        if (loadingScreen == null)
        {
            GameObject loadingPrefab = Resources.Load<GameObject>("Prefabs/UI/Loading Screen");
            if (loadingPrefab != null)
            {
                loadingScreen = Instantiate(loadingPrefab);
                loadingScreen.name = "Loading Screen";
                DontDestroyOnLoad(loadingScreen);
            }
            else
            {
                Debug.LogError("Loading Screen �������� ã�� �� �����ϴ�. Resources ������ Ȯ���ϼ���.");
            }
        }

        // �ε� ȭ���� ������Ʈ �Ҵ�
        if (progressSlider == null && loadingScreen != null)
        {
            progressSlider = loadingScreen.transform.Find("Progress Slider")?.GetComponent<Slider>();
        }

        if (progressText == null && loadingScreen != null)
        {
            progressText = loadingScreen.transform.Find("Progress Txt")?.GetComponent<Text>();
        }
    }

    private IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            Debug.Log($"�ε� ��: {operation.progress * 100}%");
            UpdateProgress(operation.progress);
            yield return null;
        }

        Debug.Log("90% �ε� �Ϸ�, �߰� ��� ��...");
        // 90% ���� ��, �Ϸ� ǥ��
        UpdateProgress(1f);
        yield return new WaitForSeconds(1f);  // �ʿ��� ���, �߰� ��� �ð�

        Debug.Log("�� Ȱ��ȭ ��� ��...");
        operation.allowSceneActivation = true;  // �� Ȱ��ȭ ���

        while (!operation.isDone)
        { 
            yield return null;
        }

        Debug.Log("�� ��ȯ �Ϸ�");
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
            Debug.Log("LoadingScreen ��Ȱ��ȭ �Ϸ�");
        }
    }

    private void UpdateProgress(float progress)
    {
        float displayProgress = Mathf.Clamp01(progress / 0.9f);

        if (progressSlider != null)
        {
            progressSlider.value = displayProgress;
        }

        if (progressText != null)
        {
            progressText.text = Mathf.RoundToInt(displayProgress * 100f) + "%";
        }
    }

    public void Clear()
    {

    }
}
