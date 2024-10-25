using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpecitficObject : InteractableObjExtand
{
    [SerializeField] private string gameClearPanelPrefabPath = "Prefabs/UI/WhetherMissionCompleted";
    [SerializeField] private GameObject missionClearPanel; // 미션 클리어 UI
    [SerializeField] private string targetSceneName = "MainMenuScene"; // 전환할 씬 이름
    [SerializeField] private float triggerDistance = 5f; // 트리거 범위
    // [SerializeField] private int sortingOrder = 100; // 다른 UI를 덮을 정렬 순서

    Transform player;
    private Button quitButton; // Quit 버튼
    private GameObject missionCompletedPanel;
    private Canvas clearPanelCanvas;

    private bool isUIShown = false;

    void Start()
    {
        gameObject.SetActive(false);

        // 플레이어가 null인지 체크하고 자동으로 할당
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

            if (player == null)
            {
                Debug.LogError("Player가 설정되지 않았습니다. Inspector에서 설정하거나 태그를 확인하세요.");
            }

        }
        // 미션 클리어 UI 미리 로드
        LoadUI();

    }

    private void Update()
    {
        if (player == null) return; // 플레이어가 없을 경우 Update 중지

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 특정 거리 이내로 플레이어가 접근했을 때 UI를 표시
        if (distanceToPlayer <= triggerDistance && !isUIShown)
        {
            ShowGameClearPanel();
        }
    }

    public override void Interaction()
    {
        if (!interactable) return;

        // 미션 클리어 UI 표시
        ShowGameClearPanel();
    }

    private void LoadUI()
    {
        if (missionClearPanel == null)
        {
            GameObject gameClearPrefab = Resources.Load<GameObject>(gameClearPanelPrefabPath);
            if (gameClearPrefab != null)
            {
                missionClearPanel = Instantiate(gameClearPrefab, transform);
                Debug.Log("gameClearPanel 프리팹 로드 성공");

                // 정확한 경로로 MissionCompleted_Panel을 찾음
                Transform canvasTransform = missionClearPanel.transform.Find("Canvas");
                if (canvasTransform != null)
                {
                    missionCompletedPanel = canvasTransform.Find("MissionCompleted_Panel")?.gameObject;
                    if (missionCompletedPanel != null)
                    {
                        Debug.Log("MissionCompleted_Panel을 성공적으로 찾았습니다.");
                        missionCompletedPanel.SetActive(false); // 초기에는 비활성화

                        // RectTransform으로 UI 위치 조정
                        RectTransform rectTransform = missionCompletedPanel.GetComponent<RectTransform>();
                        if (rectTransform != null)
                        {
                            // Anchors를 중앙에 설정
                            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                            rectTransform.pivot = new Vector2(0.5f, 0.5f);

                            // 초기 위치 설정 (화면의 중앙)
                            rectTransform.anchoredPosition = Vector2.zero;
                        }
                    }
                    else
                    {
                        Debug.LogError("MissionCompleted_Panel을 찾을 수 없습니다.");
                    }
                }
                else
                {
                    Debug.LogError("Canvas를 찾을 수 없습니다.");
                }
            }
            else
            {
                Debug.LogError($"프리팹을 {gameClearPanelPrefabPath} 경로에서 로드할 수 없습니다.");
            }
        }
    }

    private void ShowGameClearPanel()
    {
        // 게임 클리어 패널을 활성화
        if (missionCompletedPanel != null)
        {
            missionCompletedPanel.SetActive(true);
            isUIShown = true; // UI가 한 번만 나타나도록 설정
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

        missionClearPanel.SetActive(true); // 전체 패널 활성화
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

}
