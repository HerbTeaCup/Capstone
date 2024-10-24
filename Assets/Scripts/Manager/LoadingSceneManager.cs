using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    [Header("Loading UI")]
    public GameObject loadingScreen;  // �ε� ȭ�� ������Ʈ
    public Slider progressSlider;     // �ε� ���� ��
    public Text progressText;         // �ۼ�Ƽ�� �ؽ�Ʈ

    private static LoadingSceneManager _instance = GameManager.LoadingScene;

    private void Start()
    {
        // �ߺ� ���� ����
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            EnsureUIComponents();
            Debug.Log("LoadingSceneManager �ν��Ͻ��� �����Ǿ����ϴ�.");
        }
        else if (_instance != this)
        {
            Destroy(gameObject);  // �ߺ��� �ν��Ͻ��� ����
            Debug.LogWarning("LoadingSceneManager �ߺ� ������ �����Ǿ� �ı��Ǿ����ϴ�.");
            return;
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

    public void LoadSceneWithInit(int sceneIndex)
    {
        if (SceneManager.GetActiveScene().buildIndex == sceneIndex)
        {
            Debug.LogWarning("�̹� Ȱ��ȭ�� ���Դϴ�.");
            return;
        }
        StartCoroutine(InitializeAndLoadScene(sceneIndex));
    }

    private IEnumerator InitializeAndLoadScene(int sceneIndex)
    {
        // �� ��ȯ �� Time.timeScale�� ����
        Time.timeScale = 1;

        // ���� ���� ������Ʈ�� �ʱ�ȭ �� ����
        yield return StartCoroutine(CleanupCurrentScene());

        // �ε� ����
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }
        yield return StartCoroutine(LoadAsynchronously(sceneIndex));

        // ���� �ε�� �� �÷��̾� ���� �ʱ�ȭ (�Է� ���� �ݹ� ��)
        var player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            var playerInput = player.GetComponent<PlayerInput>();
            if (playerInput != null)
            {
                // �׼� �� Ȱ��ȭ ���� InputActionAsset�� "Gameplay"�� �ִ��� Ȯ��
                if (playerInput.actions.FindActionMap("Gameplay") != null)
                {
                    playerInput.SwitchCurrentActionMap("Gameplay");  // �׼� �� Ȱ��ȭ
                    Debug.Log("Gameplay �׼� ������ ��ȯ�Ǿ����ϴ�.");
                }
                else
                {
                    Debug.LogError("Gameplay �׼� ���� ã�� �� �����ϴ�.");
                }
            }
            else
            {
                Debug.LogError("PlayerInput�� ������� �ʾҽ��ϴ�.");
            }
        }
    }

    // ���� ���� Ȱ��ȭ�� ��� ������Ʈ�� �ʱ�ȭ �� ����
    private IEnumerator CleanupCurrentScene()
    {
        Debug.Log("�� �ʱ�ȭ ��...");
        GameManager.Instance.Clear(); // ��� �Ŵ����� Clear �޼��� ȣ��

        GameObject player = GameObject.FindWithTag("Player");

        // ���� ��Ʈ ������Ʈ ��Ȱ��ȭ
        var rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (var obj in rootGameObjects)
        {
            if (obj != gameObject && obj != player) // LoadingSceneManager�� ����
            {
                obj.SetActive(false);
            }
        }

        yield return new WaitForSeconds(0.5f); // ��� ���
    }


    private void EnsureUIComponents()
    {
        // �ε� ȭ���� ã��, ������ Resources���� �ε�
        if (loadingScreen == null)
        {
            GameObject loadingPrefab = Resources.Load<GameObject>("Prefabs/UI/Loading Screen"); // ResourceManager ����ؾ� ��
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

        // 90% ���� ��, �Ϸ� ǥ��
        UpdateProgress(1f);
        Debug.Log("90% �ε� �Ϸ�, �߰� ��� ��...");

        // 1�� ��� �� �� Ȱ��ȭ
        yield return new WaitForSeconds(1f);
        operation.allowSceneActivation = true;  // �� Ȱ��ȭ ���

        // ���� �ε�� ������ ���
        while (!operation.isDone)
        {
            yield return null;
        }

        Debug.Log("�� ��ȯ �Ϸ�");

        // �� ��ȯ�� �Ϸ�Ǹ� �ε� ȭ�� ��Ȱ��ȭ
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
        // ���߿� �ʿ� �� �߰�
    }
}
