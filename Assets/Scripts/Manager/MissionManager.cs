using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public delegate void UpdateAction();
    public event UpdateAction UpdateDelegate;

    [Tooltip("�̼� ������Ʈ ����Ʈ")]
    [SerializeField] private List<StageInteractiveObj> missionList = new List<StageInteractiveObj>(); // �̼� ������Ʈ ����Ʈ

    private int currentMissionIndex = 0; // ���� Ȱ��ȭ�� �̼��� �ε���

    public void Updater()
    {
        if (UpdateDelegate != null)
        {
            UpdateDelegate.Invoke();
        }
    }

    public void Clear()
    {

    }

    public void InitializeMissions()
    {
        // ù ��° �̼� Ȱ��ȭ
        if (missionList.Count > 0)
        {
            ActivateMission(0); // ù ��° �̼� Ȱ��ȭ
        }
    }

    // �̼� Ȱ��ȭ
    public void ActivateMission(int index)
    {
        if (index < 0 || index >= missionList.Count)
        {
            Debug.LogWarning("�߸��� �̼� �ε����Դϴ�.");
            return;
        }

        // ���� Ȱ��ȭ�� �̼� ��Ȱ��ȭ
        if (currentMissionIndex < missionList.Count)
        {
            missionList[currentMissionIndex].gameObject.SetActive(false);
        }

        // ���ο� �̼� Ȱ��ȭ
        missionList[index].gameObject.SetActive(true);
        currentMissionIndex = index;
    }

    // ���� �̼� Ȱ��ȭ
    public void ActivateNextMission(StageInteractiveObj currentMission)
    {
        int nextMissionIndex = missionList.IndexOf(currentMission) + 1;
        if (nextMissionIndex < missionList.Count)
        {
            ActivateMission(nextMissionIndex);
        }
        else
        {
            Debug.Log("��� �̼��� �Ϸ��߽��ϴ�!");
        }
    }

    // ���� �̼��� UI Ȱ��ȭ
    public void ActivateNextMissionUI()
    {
        MissionObjIndicator.ShowNextMissionUI(); // UI ��ȯ
    }

    // ���� �̼� ������ �̼��� ��ȯ
    public StageInteractiveObj GetNextMission()
    {
        int nextMissionIndex = currentMissionIndex + 1;
        if (nextMissionIndex < missionList.Count)
        {
            return missionList[nextMissionIndex];
        }
        return null; // �� �̻� ���� �̼��� ���� ��� null ��ȯ
    }

    public void DeactivateCurrentMission()
    {
        if (currentMissionIndex < missionList.Count)
        {
            missionList[currentMissionIndex].gameObject.SetActive(false);
        }
    }
}
