using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public delegate void UpdateAction();
    public event UpdateAction UpdateDelegate;

    [Tooltip("미션 오브젝트 리스트")]
    [SerializeField] private List<StageInteractiveObj> missionList = new List<StageInteractiveObj>(); // 미션 오브젝트 리스트

    private int currentMissionIndex = 0; // 현재 활성화된 미션의 인덱스

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
        // 첫 번째 미션 활성화
        if (missionList.Count > 0)
        {
            ActivateMission(0); // 첫 번째 미션 활성화
        }
    }

    // 미션 활성화
    public void ActivateMission(int index)
    {
        if (index < 0 || index >= missionList.Count)
        {
            Debug.LogWarning("잘못된 미션 인덱스입니다.");
            return;
        }

        // 현재 활성화된 미션 비활성화
        if (currentMissionIndex < missionList.Count)
        {
            missionList[currentMissionIndex].gameObject.SetActive(false);
        }

        // 새로운 미션 활성화
        missionList[index].gameObject.SetActive(true);
        currentMissionIndex = index;
    }

    // 다음 미션 활성화
    public void ActivateNextMission(StageInteractiveObj currentMission)
    {
        int nextMissionIndex = missionList.IndexOf(currentMission) + 1;
        if (nextMissionIndex < missionList.Count)
        {
            ActivateMission(nextMissionIndex);
        }
        else
        {
            Debug.Log("모든 미션을 완료했습니다!");
        }
    }

    // 다음 미션의 UI 활성화
    public void ActivateNextMissionUI()
    {
        MissionObjIndicator.ShowNextMissionUI(); // UI 전환
    }

    // 현재 미션 이후의 미션을 반환
    public StageInteractiveObj GetNextMission()
    {
        int nextMissionIndex = currentMissionIndex + 1;
        if (nextMissionIndex < missionList.Count)
        {
            return missionList[nextMissionIndex];
        }
        return null; // 더 이상 남은 미션이 없을 경우 null 반환
    }

    public void DeactivateCurrentMission()
    {
        if (currentMissionIndex < missionList.Count)
        {
            missionList[currentMissionIndex].gameObject.SetActive(false);
        }
    }
}
