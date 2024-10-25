using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MissionManager;

public class MissionManager : MonoBehaviour
{
    [Tooltip("�̼� ������Ʈ ����Ʈ")]
    [SerializeField] private List<StageInteractiveObj> missionList = new List<StageInteractiveObj>();
    [SerializeField] public List<EnemyStatus> targetEnemies = new List<EnemyStatus>();

    [SerializeField] private ClearInteractiveObj clearInteractiveObj;

    public event Action<int> RemainingEnemiesUpdated;

    private MissionObjIndicator missionObjIndicator;

    private int currentMissionIndex = 0;
    public int eliminatedTargetCount = 0;

    private void Start()
    {
        InitializeMissions();

        UpdateRemainingEnemiesUI();
    }

    public void InitializeMissions()
    {
        eliminatedTargetCount = 0;

        UpdateRemainingEnemiesUI();

        // �̼� ����Ʈ�� Ÿ�� �� ����Ʈ�� ��� �ִ��� Ȯ��
        if (missionList.Count == 0 && targetEnemies.Count == 0)
        {
            Debug.LogWarning("�̼� �Ǵ� Ÿ�� ���� �����ϴ�.");
            return;
        }

        MissionObjIndicator missionIndicator = FindObjectOfType<MissionObjIndicator>();

        // ClearInteractiveObj�� �ʱ�ȭ ���·� ����
        if (clearInteractiveObj != null)
        {
            clearInteractiveObj.gameObject.SetActive(false);
        }

        // ù ��° �̼� �Ǵ� Ÿ�� �� ����
        if (missionList.Count > 0)
        {
            ActivateMission(0);
        }
    }

    public void OnTargetEnemyEliminated(EnemyStatus enemy)
    {
        if (targetEnemies.Contains(enemy))
        {
            eliminatedTargetCount++;
            Debug.Log($"{eliminatedTargetCount}/{targetEnemies.Count} Ÿ�� ���ŵ�");

            UpdateRemainingEnemiesUI();

            // missionObjIndicator�� ������ ������Ʈ
            if (missionObjIndicator != null)
            {
                missionObjIndicator.MarkObjectAsCompleted();

            }
            if (eliminatedTargetCount >= targetEnemies.Count)
            {
                Debug.Log("��� Ÿ�� ���� �Ϸ�!");
                CheckMissionCompletion();
            }
        }
    }

    private void UpdateRemainingEnemiesUI()
    {
        int remainingEnemies = targetEnemies.Count - eliminatedTargetCount;

        // �̺�Ʈ�� ���� UI�� ���� �� �� ������Ʈ
        if (RemainingEnemiesUpdated != null)
        {
            RemainingEnemiesUpdated.Invoke(remainingEnemies);
        }
    }

    public void OnMissionCompleted()
    {
        // ��ȣ�ۿ� �̼� �Ϸ� ó��
        currentMissionIndex++;
        if (currentMissionIndex < missionList.Count)
        {
            ActivateMission(currentMissionIndex);
        }
        else
        {
            Debug.Log("��� ��ȣ�ۿ� �̼� �Ϸ�!");
            CheckMissionCompletion();
        }
    }

    public void CheckMissionCompletion()
    {
        // Ÿ�� �� óġ�� ��ȣ�ۿ� �̼��� ��� Ŭ����Ǿ����� Ȯ��
        if (eliminatedTargetCount >= targetEnemies.Count && currentMissionIndex >= missionList.Count)
        {
            Debug.Log("��� �̼� �Ϸ�! ClearInteractiveObj Ȱ��ȭ!");
            ActivateClearInteractive();
        }
    }

    public void ActivateMission(int index)
    {
        if (index < 0 || index >= missionList.Count)
        {
            Debug.LogWarning("�߸��� �̼� �ε����Դϴ�.");
            return;
        }

        DeactivateAllMissions();

        missionList[index].gameObject.SetActive(true);
        missionList[index].SetInteractable(true);
        currentMissionIndex = index;

        if (missionObjIndicator != null)
        {
            missionObjIndicator.ShowMissionUIForMission(missionList[index]);
        }
    }

    private void DeactivateAllMissions()
    {
        foreach (var mission in missionList)
        {
            mission.gameObject.SetActive(false);
            mission.SetInteractable(false);
        }

        if (missionObjIndicator != null)
        {
            missionObjIndicator.HideUI();
        }
    }

    public void ActivateNextMission()
    {
        StartCoroutine(ActivateNextMissionAfterCompletion());
    }
    private IEnumerator ActivateNextMissionAfterCompletion()
    {
        // MissionObjIndicator���� MarkObjectAsCompleted ȣ�� �Ϸ���� ���
        MissionObjIndicator currentIndicator = FindObjectOfType<MissionObjIndicator>();
        if (currentIndicator != null)
        {
            while (!currentIndicator.isMissionCompleted)
            {
                yield return null; // MarkObjectAsCompleted�� �Ϸ�� ������ ���
            }
        }
        yield return new WaitForSeconds(1f);

        // ���� �̼��� ó��
        int nextMissionIndex = currentMissionIndex + 1;

        if (nextMissionIndex < missionList.Count)
        {
            // ���� �̼��� Ȱ��ȭ
            ActivateMission(nextMissionIndex);
        }
        else
        {
            Debug.Log("��� �̼��� �Ϸ��߽��ϴ�!");
            DeactivateAllMissions();
            ActivateClearInteractive();
        }
    }


    private void ActivateClearInteractive()
    {
        if (clearInteractiveObj != null)
        {
            clearInteractiveObj.gameObject.SetActive(true);
            clearInteractiveObj.interactable = true;

            if (missionObjIndicator != null)
            {
                missionObjIndicator.ShowClearInteractiveIndicator(clearInteractiveObj); // ��ġ UI ǥ��
            }

            Debug.Log("ClearInteractive ������Ʈ Ȱ��ȭ��");
        }
    }
    public void Clear()
    {

    }
}
