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

    private static LoadingSceneManager _instance;

    private void Start()
    {
        // 싱글톤 패턴 적용으로 중복 생성 방지
        if (_instance == null)
        {
            _instance = this;
            GameManager.LoadingScene = this;
            DontDestroyOnLoad(gameObject);
            EnsureUIComponents();
            Debug.Log("LoadingSceneManager 인스턴스가 생성되었습니다.");
        }
        else
        {
            Destroy(gameObject);  // 중복된 인스턴스는 삭제
            Debug.LogWarning("LoadingSceneManager 중복 생성이 감지되어 파괴되었습니다.");
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

    private void EnsureUIComponents()
    {
        // 로딩 화면을 찾고, 없으면 Resources에서 로드
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

        Debug.Log("90% 로딩 완료, 추가 대기 중...");
        // 90% 도달 후, 완료 표시
        UpdateProgress(1f);
        yield return new WaitForSeconds(1f);  // 필요한 경우, 추가 대기 시간

        Debug.Log("씬 활성화 허용 중...");
        operation.allowSceneActivation = true;  // 씬 활성화 허용

        while (!operation.isDone)
        { 
            yield return null;
        }

        Debug.Log("씬 전환 완료");
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

    }
}
