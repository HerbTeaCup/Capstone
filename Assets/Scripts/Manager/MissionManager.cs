using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public delegate void UpdateAction();
    public event UpdateAction UpdateDelegate;

    [Tooltip("�̼� ������Ʈ ����Ʈ")]
    [SerializeField] private List<StageInteractiveObj> missionList = new List<StageInteractiveObj>(); // �̼� ������Ʈ ����Ʈ

    private int currentMissionIndex = 0; // ���� Ȱ��ȭ�� �̼��� �ε���

    private void Start()
    {
        InitializeMissions();
    }
    public void Updater()
    {
        if (UpdateDelegate != null)
        {
            UpdateDelegate.Invoke();
        }
    }

        // �̼� �ʱ�ȭ (ù ��° �̼� Ȱ��ȭ)
    public void InitializeMissions()
    {
        if (missionList.Count > 0)
        {
            ActivateMission(0); // ù ��° �̼Ǹ� Ȱ��ȭ
        }
    }

    // �̼� Ȱ��ȭ (���� �ϳ��� �̼Ǹ� Ȱ��ȭ��)
    public void ActivateMission(int index)
    {
        if (index < 0 || index >= missionList.Count)
        {
            Debug.LogWarning("�߸��� �̼� �ε����Դϴ�.");
            return;
        }

        DeactivateAllMissions(); // ��� �̼� ��Ȱ��ȭ

        missionList[index].gameObject.SetActive(true); // ������ �̼� Ȱ��ȭ
        missionList[index].SetInteractable(true); // ��ȣ�ۿ� �����ϰ� ����
        currentMissionIndex = index;

        MissionObjIndicator.ShowMissionUIForMission(missionList[index]); // UI�� �ش� �̼ǿ� ���缭 ����
    }

    // ��� �̼� ��Ȱ��ȭ
    private void DeactivateAllMissions()
    {
        foreach (var mission in missionList)
        {
            mission.gameObject.SetActive(false);
            mission.SetInteractable(false); // ��ȣ�ۿ� ��Ȱ��ȭ
        }
    }

    // ���� �̼� Ȱ��ȭ
    public void ActivateNextMission()
    {
        int nextMissionIndex = currentMissionIndex + 1;

        if (nextMissionIndex < missionList.Count)
        {
            ActivateMission(nextMissionIndex); // ���� �̼� Ȱ��ȭ
        }
        else
        {
            Debug.Log("��� �̼��� �Ϸ��߽��ϴ�!");
        }
    }

    public void Clear()
    {

    }
}
