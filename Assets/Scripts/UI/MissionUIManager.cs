using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionUIManager : MonoBehaviour
{
    [Header("Main Camera")]
    [SerializeField] Camera mainCamera; // ���� ī�޶�

    [Header("Mission UI Canvas")]
    [SerializeField] string missionCanvasPrefabPath = "Prefabs/UI/Mission UI Manager";  // ĵ���� ������ ���

    [Header("Mission UI Settings")]
    [SerializeField] GameObject missionCanvas; // �̼� ĵ����
    [SerializeField] GameObject missionFailedPanel; // �̼� ���� �г�
    [SerializeField] GameObject missionCompletedPanel; // �̼� �Ϸ� �г�

    private Transform player; // �÷��̾��� Transform

    public bool isMissionCompleted = false;
    public bool isMissionFailed = false;

    // ���� �ε� UI
    // private GameObject missionFailed; // �̼� ����
    // private GameObject missionCompleted; // �̼� ����

    void Start()
    {
        // Main Camera �ڵ� ����
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("Main Camera�� ã�� �� �����ϴ�. ���� ī�޶� �������ּ���.");
                return;
            }
        }

        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player == null)
        {
            Debug.LogError("Player�� ã�� �� �����ϴ�. Player �±׸� Ȯ�����ּ���.");
        }

        LoadUI();

        GameManager.Enemy.UpdateDelegate += CheckMissionStatus;
    }

    void LoadUI()
    {
        // ���ҽ� ��ο��� ĵ���� ������ �ε�
        GameObject missionCanvasPrefab = ResourceManager.Load<GameObject>(missionCanvasPrefabPath);

        if (missionCanvasPrefab == null)
            return;

        // �������� ĵ���� ���� �� �߰�
        missionCanvas = Instantiate(missionCanvasPrefab, transform);

        // �ڽ� ������Ʈ���� �г��� ã�� ����
        Transform missionCanvasTransform = missionCanvas.transform.Find("Canvas");
        if (missionCanvasTransform == null)
        {
            Debug.LogError("Canvas ������Ʈ�� ã�� �� �����ϴ�. Mission UI�� ������ Ȯ�����ּ���.");
            return;
        }

        missionFailedPanel = missionCanvasTransform.Find("MissionFailed_Panel")?.gameObject;
        missionCompletedPanel = missionCanvasTransform.Find("MissionCompleted_Panel")?.gameObject;

        if (missionFailedPanel != null)
        {
           missionFailedPanel.SetActive(false);  // �ʱ⿡�� ��Ȱ��ȭ
        }
        else
        {
            Debug.LogError("MissionFailed_Panel�� ã�� �� �����ϴ�. ��θ� Ȯ�����ּ���.");
        }

        if (missionCompletedPanel != null)
        {
            missionCompletedPanel.SetActive(false);  // �ʱ⿡�� ��Ȱ��ȭ
        }
        else
        {
            Debug.LogError("MissionCompleted_Panel�� ã�� �� �����ϴ�. ��θ� Ȯ�����ּ���.");
        }
    }

    void CheckMissionStatus()
    {
        // �÷��̾ ���� ��� �̼� ����
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

        // ��� Ÿ���� ���ŵ� ��� �̼� ����
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
            missionFailedPanel.SetActive(true); // �̼� ���� UI Ȱ��ȭ
        }
    }

    void ShowMissionCompletedPanel()
    {
        if (missionCompletedPanel != null)
        {
            missionCompletedPanel.SetActive(true); // �̼� �Ϸ� UI Ȱ��ȭ
        }
    }

    // �ش� �ڵ尡 �־���� �ǰ� 0%�� �Ǵ� �� �� ���Ŀ� �̼� ���� â �˾�
    IEnumerator PauseGameAfterDelay()
    {
        yield return new WaitForSeconds(1f); // 1�� ��� ��
        PauseGame();
    }

    void PauseGame()
    {
        Time.timeScale = 0f; // ���� �Ͻ�����

        Canvas[] allCanvas = FindObjectsOfType<Canvas>();
        foreach (Canvas canvas in allCanvas)
        {
            if (canvas.gameObject != missionCanvas)
            {
                canvas.enabled = false;
            }
        }

        // �̼� �гθ� Ȱ��ȭ
        if (isMissionFailed && missionFailedPanel != null)
        {
            missionFailedPanel.SetActive(true);
        }
        
        if (isMissionCompleted && missionCompletedPanel != null)
        {
            missionCompletedPanel.SetActive(true);
        }

        // Mission UI�� Canvas�� Ȱ��ȭ
        Canvas missionUICanvas = missionCanvas.GetComponentInChildren<Canvas>();
        if (missionUICanvas != null)
        {
            missionUICanvas.enabled = true;
        }
    }
}
