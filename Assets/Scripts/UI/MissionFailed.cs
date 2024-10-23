using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MissionFailed : MonoBehaviour
{
    [Header("Main Camera")]
    [SerializeField] Camera mainCamera; // 메인 카메라

    [Header("Mission Failed UI")]
    [SerializeField] string missionCanvasPrefabPath = "Prefabs/UI/WhetherMissionCompleted";  // 캔버스 프리팹 경로
    [SerializeField] GameObject missionCanvas; // 동적 생성된 미션 캔버스
    [SerializeField] GameObject missionFailedPanel; // 미션 실패 패널

    private Button quitButton; // Quit 버튼
    private Button restartButton; // Restart 버튼

    private PlayerStatus _player;
    private bool isMissionFailed = false;

    void Start()
    {
        // Main Camera 자동 설정
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("Main Camera를 찾을 수 없습니다. 메인 카메라를 설정해주세요.");
                return;
            }
        }

        // PlayerStatus 초기화
        _player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerStatus>();

        if (_player == null)
        {
            Debug.LogError("Player를 찾을 수 없거나 PlayerStatus 컴포넌트가 없습니다.");
            return;
        }

        LoadUI();
    }

    void Update()
    {
        // 플레이어가 사망한 경우 미션 실패 창 표시
        if (_player != null && _player.IsAlive == false)
        {
            if (!isMissionFailed)
            {
                ShowMissionFailedPanel();
                isMissionFailed = true;
                StartCoroutine(PauseGameAfterDelay());
            }
        }
    }

    void LoadUI()
    {
        // 리소스 경로에서 캔버스 프리팹 로드
        GameObject missionCanvasPrefab = Resources.Load<GameObject>(missionCanvasPrefabPath);

        // 프리팹이 제대로 로드되었는지 확인
        if (missionCanvasPrefab == null)
        {
            Debug.LogError($"프리팹을 {missionCanvasPrefabPath} 경로에서 찾을 수 없습니다.");
            return;
        }

        // 동적으로 캔버스 생성 및 추가
        missionCanvas = Instantiate(missionCanvasPrefab, transform);
        Debug.Log("Mission canvas instantiated.");

        // 자식 오브젝트에서 패널을 찾아 설정
        Transform missionCanvasTransform = missionCanvas.transform.Find("Canvas");
        if (missionCanvasTransform == null)
        {
            Debug.LogError("Canvas 오브젝트를 찾을 수 없습니다. Mission UI의 구조를 확인해주세요.");
            return;
        }

        missionFailedPanel = missionCanvasTransform.Find("MissionFailed_Panel")?.gameObject;

        if (missionFailedPanel != null)
        {
            missionFailedPanel.SetActive(false);  // 초기에는 비활성화
            Debug.Log("MissionFailed_Panel loaded successfully.");

            // GetComponentInChildren을 사용해 QuitButton 및 RestartButton 찾기
            quitButton = missionFailedPanel.transform.Find("QuitButton")?.GetComponent<Button>();
            restartButton = missionFailedPanel.transform.Find("RestartButton")?.GetComponent<Button>();

            // 버튼이 제대로 찾아졌는지 확인
            if (quitButton == null || restartButton == null)
            {
                Debug.LogError("QuitButton 또는 RestartButton을 찾을 수 없습니다.");
                return;
            }

            // 버튼 이름을 확인 후 각각의 이벤트 리스너 추가
            if (quitButton.name == "QuitButton")
            {
                quitButton.onClick.AddListener(QuitToMainMenu); // Quit 버튼 클릭 이벤트 등록
            }
            if (restartButton.name == "RestartButton")
            {
                restartButton.onClick.AddListener(RestartScene); // Restart 버튼 클릭 이벤트 등록
            }
        }
        else
        {
            Debug.LogError("MissionFailed_Panel을 찾을 수 없습니다. 경로를 확인해주세요.");
        }
    }

    void ShowMissionFailedPanel()
    {
        if (missionFailedPanel != null)
        {
            PauseGameAfterDelay();
            missionFailedPanel.SetActive(true); // 미션 실패 UI 활성화
            Debug.Log("MissionFailed_Panel activated.");
            Time.timeScale = 0f; // 시간정지
        }
    }

    // 피가 0%가 된 후 미션 실패 창 팝업
    IEnumerator PauseGameAfterDelay()
    {
        yield return new WaitForSeconds(3f); // 1초 대기 후
    }

    // Quit 버튼 클릭 시 메인 메뉴로 이동
    void QuitToMainMenu()
    {
        Debug.Log("Quit to Main Menu 버튼 클릭됨.");
        Time.timeScale = 1f; // 시간 다시 흐르게
        SceneManager.LoadScene(0); // 메인 메뉴 씬으로 이동
    }

    // Restart 버튼 클릭 시 현재 씬 다시 시작
    void RestartScene()
    {
        Debug.Log("Restart Scene 버튼 클릭됨.");
        Time.timeScale = 1f; // 시간 다시 흐르게
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // 현재 씬 다시 시작
    }
}
