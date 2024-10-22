using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [Tooltip("�̼� ������Ʈ ����Ʈ")]
    [SerializeField] private List<StageInteractiveObj> missionList = new List<StageInteractiveObj>();
    [SerializeField] private ClearInteractiveObj clearInteractiveObj;
    [SerializeField] private MissionObjIndicator missionObjIndicator;

    private int currentMissionIndex = 0;

    private void Start()
    {
        InitializeMissions();
    }

    public void InitializeMissions()
    {
        if (missionList.Count > 0)
        {
            ActivateMission(0);
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
        int nextMissionIndex = currentMissionIndex + 1;

        if (nextMissionIndex < missionList.Count)
        {
            ActivateMission(nextMissionIndex);
        }
        else
        {
            Debug.Log("��� �̼��� �Ϸ��߽��ϴ�!");
            ActivateClearInteractive();
        }
    }

    private void ActivateClearInteractive()
    {
        if (clearInteractiveObj != null)
        {
            clearInteractiveObj.gameObject.SetActive(true);
            clearInteractiveObj.SetInteractable(true);

            if (missionObjIndicator != null)
            {
                missionObjIndicator.ShowClearInteractiveIndicator(clearInteractiveObj);
            }

            Debug.Log("ClearInteractive ������Ʈ Ȱ��ȭ��");
        }
    }
    public void Clear()
    {

    }
}
