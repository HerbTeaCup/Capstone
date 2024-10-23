using UnityEngine;
using UnityEngine.SceneManagement; // 씬 전환을 위해 추가
using UnityEngine.UI; // UI 버튼을 다루기 위해 추가

public class ClearInteractiveObj : InteractableObjExtand
{
    [SerializeField] private string gameClearPanelPrefabPath = "Prefabs/UI/WhetherMissionCompleted";
    [SerializeField] private string targetSceneName = "MainMenuScene"; // 전환할 씬 이름
    [SerializeField] private Transform targetObject;
    [SerializeField] private float clearDistance = 5f;

    private GameObject gameClearPanel;
    private GameObject missionCompletedPanel;
    private GameObject missionFailedPanel;
    private Button quitButton; // Quit 버튼
    private Button restartButton; // Restart 버튼

    private void Start()
    {
        gameObject.SetActive(false); // 초기 비활성화
    }

    public void SetInteractable(bool value)
    {
        interactable = value;
    }

    public override void Interaction()
    {
        if (!interactable) return;

        // 대상 오브젝트와의 거리 체크
        float distanceToTarget = Vector3.Distance(transform.position, targetObject.position);
        if (distanceToTarget <= clearDistance)
        {
            ShowGameClearPanel(); // 패널 표시
        }
        else
        {
            Debug.Log("게임 클리어 위치에 있지 않음.");
        }
    }

    // 플레이어가 죽었을 때 호출
    public void PlayerDied()
    {
        ShowMissionFailedPanel();
    }

    private void ShowGameClearPanel()
    {
        if (gameClearPanel == null)
        {
            GameObject gameClearPrefab = Resources.Load<GameObject>(gameClearPanelPrefabPath);
            if (gameClearPrefab != null)
            {
                // 프리팹 생성
                gameClearPanel = Instantiate(gameClearPrefab);
                Debug.Log("게임 클리어 패널이 생성되었습니다.");
            }
            else
            {
                Debug.LogError($"경로에서 게임 클리어 패널 프리팹을 찾을 수 없습니다: {gameClearPanelPrefabPath}");
                return;
            }
        }

        // Canvas 하위 MissionCompleted_Panel 찾기
        missionCompletedPanel = gameClearPanel.transform.Find("Canvas/MissionCompleted_Panel")?.gameObject;
        if (missionCompletedPanel != null)
        {
            missionCompletedPanel.SetActive(true);
            Debug.Log("MissionCompleted_Panel이 활성화되었습니다.");

            // Quit 버튼 오브젝트 찾기
            quitButton = missionCompletedPanel.transform.Find("QuitButton")?.GetComponent<Button>();
            if (quitButton != null)
            {
                // Quit 버튼 클릭 시 씬 전환 이벤트 추가
                quitButton.onClick.AddListener(OnQuitButtonClick);
            }
            else
            {
                Debug.LogError("QuitButton 버튼을 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.LogError("MissionCompleted_Panel을 찾을 수 없습니다.");
        }

        gameClearPanel.SetActive(true);
    }

    private void ShowMissionFailedPanel()
    {
        if (gameClearPanel == null)
        {
            GameObject gameClearPrefab = Resources.Load<GameObject>(gameClearPanelPrefabPath);
            if (gameClearPrefab != null)
            {
                // 프리팹 생성
                gameClearPanel = Instantiate(gameClearPrefab);
                Debug.Log("MissionFailed_Panel이 생성되었습니다.");
            }
            else
            {
                Debug.LogError($"경로에서 게임 클리어 패널 프리팹을 찾을 수 없습니다: {gameClearPanelPrefabPath}");
                return;
            }
        }

        // Canvas 하위 MissionFailed_Panel 찾기
        missionFailedPanel = gameClearPanel.transform.Find("Canvas/MissionFailed_Panel")?.gameObject;
        if (missionFailedPanel != null)
        {
            missionFailedPanel.SetActive(true);
            Debug.Log("MissionFailed_Panel이 활성화되었습니다.");

            // Quit 버튼 오브젝트 찾기
            quitButton = missionFailedPanel.transform.Find("QuitButton")?.GetComponent<Button>();
            if (quitButton != null)
            {
                quitButton.onClick.AddListener(OnQuitButtonClick); // Quit 버튼 클릭 시 MainMenu로 이동
            }
            else
            {
                Debug.LogError("QuitButton 버튼을 찾을 수 없습니다.");
            }

            // Restart 버튼 오브젝트 찾기
            restartButton = missionFailedPanel.transform.Find("RestartButton")?.GetComponent<Button>();
            if (restartButton != null)
            {
                restartButton.onClick.AddListener(OnRestartButtonClick); // Restart 버튼 클릭 시 현재 씬 재시작
            }
            else
            {
                Debug.LogError("RestartButton 버튼을 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.LogError("MissionFailed_Panel을 찾을 수 없습니다.");
        }

        gameClearPanel.SetActive(true);
    }

    // Quit 버튼 클릭 시 씬 전환 처리
    private void OnQuitButtonClick()
    {
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName); // MainMenuScene으로 전환
            Debug.Log($"{targetSceneName} 씬으로 전환 중...");
        }
        else
        {
            Debug.LogError("전환할 씬 이름이 설정되지 않았습니다.");
        }
    }

    // Restart 버튼 클릭 시 현재 씬 재시작 처리
    private void OnRestartButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 현재 씬을 재시작
        Debug.Log("현재 씬을 재시작합니다.");
    }
}
