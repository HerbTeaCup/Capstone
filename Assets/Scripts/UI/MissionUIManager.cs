using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionUIManager : MonoBehaviour
{
    [Header("Main Camera")]
    [SerializeField] Camera mainCamera; // ���� ī�޶�

    [Header("Mission UI Canvas")]
    [SerializeField] string missionCanvasPrefabPath = "Prefabs/UI/Mission UI";  // ĵ���� ������ ���

    [Header("Mission UI Settings")]
    [SerializeField] GameObject missionCanvas; // �̼� ĵ����
    [SerializeField] GameObject missionFailedPanel; // �̼� ���� �г�
    [SerializeField] GameObject missionCompletedPanel; // �̼� �Ϸ� �г�

    private Transform player; // �÷��̾��� Transform

    public bool isMissionCompleted = false;
    public bool isMissionFailed = false;

    // ���� �ε� UI
    private GameObject missionFailed; // �̼� ����
    private GameObject missionCompleted; // �̼� ����

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

        // �������� ĵ���� ���� �� �߰�
        missionCanvas = Instantiate(missionCanvasPrefab, transform);

        // �ڽ� ������Ʈ���� �г��� ã�� ����
        Transform missionCanvasTransform = missionCanvas.transform.Find("Canvas");
        if (missionCanvasTransform == null)
        {
            Debug.LogError("Canvas ������Ʈ�� ã�� �� �����ϴ�. Mission UI�� ������ Ȯ�����ּ���.");
            return;
        }

        missionFailed = missionCanvasTransform.Find("MissionFailed_Panel").gameObject;
        missionCompleted = missionCanvasTransform.Find("MissionCompleted_Panel").gameObject;

        if (missionFailed != null)
        {
            missionFailed.SetActive(false);  // �ʱ⿡�� ��Ȱ��ȭ
        }
        else
        {
            Debug.LogError("MissionFailed_Panel�� ã�� �� �����ϴ�. ��θ� Ȯ�����ּ���.");
        }

        if (missionCompleted != null)
        {
            missionCompleted.SetActive(false);  // �ʱ⿡�� ��Ȱ��ȭ
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
}
