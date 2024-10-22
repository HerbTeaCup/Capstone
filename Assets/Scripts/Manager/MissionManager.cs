using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public delegate void UpdateAction();
    public event UpdateAction UpdateDelegate;

    [Tooltip("미션 오브젝트 리스트")]
    [SerializeField] private List<StageInteractiveObj> missionList = new List<StageInteractiveObj>(); // 미션 오브젝트 리스트

    private int currentMissionIndex = 0; // 현재 활성화된 미션의 인덱스

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

        // 미션 초기화 (첫 번째 미션 활성화)
    public void InitializeMissions()
    {
        if (missionList.Count > 0)
        {
            ActivateMission(0); // 첫 번째 미션만 활성화
        }
    }

    // 미션 활성화 (오직 하나의 미션만 활성화됨)
    public void ActivateMission(int index)
    {
        if (index < 0 || index >= missionList.Count)
        {
            Debug.LogWarning("잘못된 미션 인덱스입니다.");
            return;
        }

        DeactivateAllMissions(); // 모든 미션 비활성화

        missionList[index].gameObject.SetActive(true); // 선택한 미션 활성화
        missionList[index].SetInteractable(true); // 상호작용 가능하게 설정
        currentMissionIndex = index;

        MissionObjIndicator.ShowMissionUIForMission(missionList[index]); // UI도 해당 미션에 맞춰서 변경
    }

    // 모든 미션 비활성화
    private void DeactivateAllMissions()
    {
        foreach (var mission in missionList)
        {
            mission.gameObject.SetActive(false);
            mission.SetInteractable(false); // 상호작용 비활성화
        }
    }

    // 다음 미션 활성화
    public void ActivateNextMission()
    {
        int nextMissionIndex = currentMissionIndex + 1;

        if (nextMissionIndex < missionList.Count)
        {
            ActivateMission(nextMissionIndex); // 다음 미션 활성화
        }
        else
        {
            Debug.Log("모든 미션을 완료했습니다!");
        }
    }

    public void Clear()
    {

    }
}
