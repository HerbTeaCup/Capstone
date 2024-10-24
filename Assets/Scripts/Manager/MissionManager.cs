using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [Tooltip("미션 오브젝트 리스트")]
    [SerializeField] private List<StageInteractiveObj> missionList = new List<StageInteractiveObj>();
    [SerializeField] private ClearInteractiveObj clearInteractiveObj;
    private MissionObjIndicator missionObjIndicator;

    private int currentMissionIndex = 0;

    private void Start()
    {
        InitializeMissions();
    }

    public void InitializeMissions()
    {
        clearInteractiveObj.gameObject.SetActive(false);
        if (missionList.Count > 0)
        {
            ActivateMission(0);
        }
    }

    public void ActivateMission(int index)
    {
        if (index < 0 || index >= missionList.Count)
        {
            Debug.LogWarning("잘못된 미션 인덱스입니다.");
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
        // MissionObjIndicator에서 MarkObjectAsCompleted 호출 완료까지 대기
        MissionObjIndicator currentIndicator = FindObjectOfType<MissionObjIndicator>();
        if (currentIndicator != null)
        {
            while (!currentIndicator.isMissionCompleted)
            {
                yield return null; // MarkObjectAsCompleted가 완료될 때까지 대기
            }
        }
        yield return new WaitForSeconds(1f);

        // 다음 미션을 처리
        int nextMissionIndex = currentMissionIndex + 1;

        if (nextMissionIndex < missionList.Count)
        {
            // 다음 미션을 활성화
            ActivateMission(nextMissionIndex);
        }
        else
        {
            Debug.Log("모든 미션을 완료했습니다!");
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
                missionObjIndicator.ShowClearInteractiveIndicator(clearInteractiveObj); // 위치 UI 표시
            }

            Debug.Log("ClearInteractive 오브젝트 활성화됨");
        }
    }
    public void Clear()
    {

    }
}
