using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionUIManager : MonoBehaviour
{
    [Header("Main Camera")]
    [SerializeField] Camera mainCamera; // 메인 카메라

    [Header("Mission UI Canvas")]
    [SerializeField] string missionCanvasPrefabPath = "Prefabs/UI/Mission UI Manager";  // 캔버스 프리팹 경로

    [Header("Mission UI Settings")]
    [SerializeField] GameObject missionCanvas; // 미션 캔버스
    [SerializeField] GameObject missionFailedPanel; // 미션 실패 패널
    [SerializeField] GameObject missionCompletedPanel; // 미션 완료 패널

    private Transform player; // 플레이어의 Transform

    public bool isMissionCompleted = false;
    public bool isMissionFailed = false;

    // 동적 로드 UI
    // private GameObject missionFailed; // 미션 실패
    // private GameObject missionCompleted; // 미션 성공

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

        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player == null)
        {
            Debug.LogError("Player를 찾을 수 없습니다. Player 태그를 확인해주세요.");
        }

        LoadUI();

        GameManager.Enemy.UpdateDelegate += CheckMissionStatus;
    }

    void LoadUI()
    {
        // 리소스 경로에서 캔버스 프리팹 로드
        GameObject missionCanvasPrefab = ResourceManager.Load<GameObject>(missionCanvasPrefabPath);

        if (missionCanvasPrefab == null)
            return;

        // 동적으로 캔버스 생성 및 추가
        missionCanvas = Instantiate(missionCanvasPrefab, transform);

        // 자식 오브젝트에서 패널을 찾아 설정
        Transform missionCanvasTransform = missionCanvas.transform.Find("Canvas");
        if (missionCanvasTransform == null)
        {
            Debug.LogError("Canvas 오브젝트를 찾을 수 없습니다. Mission UI의 구조를 확인해주세요.");
            return;
        }

        missionFailedPanel = missionCanvasTransform.Find("MissionFailed_Panel")?.gameObject;
        missionCompletedPanel = missionCanvasTransform.Find("MissionCompleted_Panel")?.gameObject;

        if (missionFailedPanel != null)
        {
           missionFailedPanel.SetActive(false);  // 초기에는 비활성화
        }
        else
        {
            Debug.LogError("MissionFailed_Panel을 찾을 수 없습니다. 경로를 확인해주세요.");
        }

        if (missionCompletedPanel != null)
        {
            missionCompletedPanel.SetActive(false);  // 초기에는 비활성화
        }
        else
        {
            Debug.LogError("MissionCompleted_Panel을 찾을 수 없습니다. 경로를 확인해주세요.");
        }
    }

    void CheckMissionStatus()
    {
        // 플레이어가 죽은 경우 미션 실패
        if (player == null || player.GetComponent<PlayerStatus>().IsAlive == false)
        {
            if (!isMissionFailed)
            {
                ShowMissionFailedPanel();
                isMissionFailed = true;
                StartCoroutine(PauseGameAfterDelay());
            }
            return;
        }

        // 모든 타겟이 제거된 경우 미션 성공
        if (AreAllTargetsDefeated())
        {
            if (!isMissionCompleted)
            {
                ShowMissionCompletedPanel();
                isMissionCompleted = true;
                PauseGame();
            }
        }
    }

    bool AreAllTargetsDefeated()
    {
        EnemyStatus[] enemies = FindObjectsOfType<EnemyStatus>();
        foreach (EnemyStatus enemy in enemies)
        {
            if (enemy.IsAlive)
            {
                return false;
            }
        }
        return true;
    }

    void ShowMissionFailedPanel()
    {
        if (missionFailedPanel != null)
        {
            missionFailedPanel.SetActive(true); // 미션 실패 UI 활성화
        }
    }

    void ShowMissionCompletedPanel()
    {
        if (missionCompletedPanel != null)
        {
            missionCompletedPanel.SetActive(true); // 미션 완료 UI 활성화
        }
    }

    // 해당 코드가 있어야지 피가 0%가 되는 걸 본 이후에 미션 실패 창 팝업
    IEnumerator PauseGameAfterDelay()
    {
        yield return new WaitForSeconds(1f); // 1초 대기 후
        PauseGame();
    }

    void PauseGame()
    {
        Time.timeScale = 0f; // 게임 일시정지

        Canvas[] allCanvas = FindObjectsOfType<Canvas>();
        foreach (Canvas canvas in allCanvas)
        {
            if (canvas.gameObject != missionCanvas)
            {
                canvas.enabled = false;
            }
        }

        // 미션 패널만 활성화
        if (isMissionFailed && missionFailedPanel != null)
        {
            missionFailedPanel.SetActive(true);
        }
        
        if (isMissionCompleted && missionCompletedPanel != null)
        {
            missionCompletedPanel.SetActive(true);
        }

        // Mission UI의 Canvas도 활성화
        Canvas missionUICanvas = missionCanvas.GetComponentInChildren<Canvas>();
        if (missionUICanvas != null)
        {
            missionUICanvas.enabled = true;
        }
    }
}
