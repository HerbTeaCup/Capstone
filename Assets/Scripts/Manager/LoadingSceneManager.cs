using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneManager : MonoBehaviour
{
    [Header("Loading UI")]
    public GameObject loadingScreen;  // 로딩 화면 오브젝트
    public Slider progressSlider;     // 로딩 진행 바
    public Text progressText;         // 퍼센티지 텍스트

    private static LoadingSceneManager _instance = GameManager.LoadingScene;

    private void Start()
    {
        // 중복 생성 방지
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            EnsureUIComponents();
            Debug.Log("LoadingSceneManager 인스턴스가 생성되었습니다.");
        }
        else if (_instance != this)
        {
            Destroy(gameObject);  // 중복된 인스턴스는 삭제
            Debug.LogWarning("LoadingSceneManager 중복 생성이 감지되어 파괴되었습니다.");
            return;
        }

        // 처음에는 로딩 화면 비활성화
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
        }
    }

    // 씬을 비동기로 로드하고 로딩 화면을 표시
    public void LoadScene(int sceneIndex)
    {
        if (SceneManager.GetActiveScene().buildIndex == sceneIndex)
        {
            Debug.LogWarning("이미 활성화된 씬입니다.");
            return;
        }

        EnsureUIComponents();

        // 로딩 화면 활성화
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
            Debug.Log("Loading Screen 활성화");
        }
        else
        {
            Debug.LogError("Loading Screen이 존재하지 않습니다.");
        }

        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    public void LoadSceneWithInit(int sceneIndex)
    {
        if (SceneManager.GetActiveScene().buildIndex == sceneIndex)
        {
            Debug.LogWarning("이미 활성화된 씬입니다.");
            return;
        }
        StartCoroutine(InitializeAndLoadScene(sceneIndex));
    }

    private IEnumerator InitializeAndLoadScene(int sceneIndex)
    {
        // 씬 전환 전 Time.timeScale을 복구
        Time.timeScale = 1;

        // 현재 씬의 오브젝트들 초기화 및 정리
        yield return StartCoroutine(CleanupCurrentScene());

        // 로딩 시작
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }
        yield return StartCoroutine(LoadAsynchronously(sceneIndex));

        // 씬이 로드된 후 플레이어 관련 초기화 (입력 관련 콜백 등)
        var player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            var playerInput = player.GetComponent<PlayerInput>();
            if (playerInput != null)
            {
                // 액션 맵 활성화 전에 InputActionAsset에 "Gameplay"가 있는지 확인
                if (playerInput.actions.FindActionMap("Gameplay") != null)
                {
                    playerInput.SwitchCurrentActionMap("Gameplay");  // 액션 맵 활성화
                    Debug.Log("Gameplay 액션 맵으로 전환되었습니다.");
                }
                else
                {
                    Debug.LogError("Gameplay 액션 맵을 찾을 수 없습니다.");
                }
            }
            else
            {
                Debug.LogError("PlayerInput이 연결되지 않았습니다.");
            }
        }
    }

    // 현재 씬의 활성화된 모든 오브젝트를 초기화 및 정리
    private IEnumerator CleanupCurrentScene()
    {
        Debug.Log("씬 초기화 중...");
        GameManager.Instance.Clear(); // 모든 매니저의 Clear 메서드 호출

        GameObject player = GameObject.FindWithTag("Player");

        // 씬의 루트 오브젝트 비활성화
        var rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (var obj in rootGameObjects)
        {
            if (obj != gameObject && obj != player) // LoadingSceneManager는 제외
            {
                obj.SetActive(false);
            }
        }

        yield return new WaitForSeconds(0.5f); // 잠시 대기
    }


    private void EnsureUIComponents()
    {
        // 로딩 화면을 찾고, 없으면 Resources에서 로드
        if (loadingScreen == null)
        {
            GameObject loadingPrefab = Resources.Load<GameObject>("Prefabs/UI/Loading Screen"); // ResourceManager 사용해야 함
            if (loadingPrefab != null)
            {
                loadingScreen = Instantiate(loadingPrefab);
                loadingScreen.name = "Loading Screen";
                DontDestroyOnLoad(loadingScreen);
            }
            else
            {
                Debug.LogError("Loading Screen 프리팹을 찾을 수 없습니다. Resources 폴더를 확인하세요.");
            }
        }

        // 로딩 화면의 컴포넌트 할당
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
            Debug.Log($"로딩 중: {operation.progress * 100}%");
            UpdateProgress(operation.progress);
            yield return null;
        }

        // 90% 도달 후, 완료 표시
        UpdateProgress(1f);
        Debug.Log("90% 로딩 완료, 추가 대기 중...");

        // 1초 대기 후 씬 활성화
        yield return new WaitForSeconds(1f);
        operation.allowSceneActivation = true;  // 씬 활성화 허용

        // 씬이 로드될 때까지 대기
        while (!operation.isDone)
        {
            yield return null;
        }

        Debug.Log("씬 전환 완료");

        // 씬 전환이 완료되면 로딩 화면 비활성화
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
            Debug.Log("LoadingScreen 비활성화 완료");
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
        // 나중에 필요 시 추가
    }
}
