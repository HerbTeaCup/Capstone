using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MissionManager;

public class MissionManager : MonoBehaviour
{
    [Tooltip("미션 오브젝트 리스트")]
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

        // 미션 리스트와 타겟 적 리스트가 비어 있는지 확인
        if (missionList.Count == 0 && targetEnemies.Count == 0)
        {
            Debug.LogWarning("미션 또는 타겟 적이 없습니다.");
            return;
        }

        MissionObjIndicator missionIndicator = FindObjectOfType<MissionObjIndicator>();

        // ClearInteractiveObj는 초기화 상태로 설정
        if (clearInteractiveObj != null)
        {
            clearInteractiveObj.gameObject.SetActive(false);
        }

        // 첫 번째 미션 또는 타겟 적 시작
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
            Debug.Log($"{eliminatedTargetCount}/{targetEnemies.Count} 타겟 제거됨");

            UpdateRemainingEnemiesUI();

            // missionObjIndicator가 있으면 업데이트
            if (missionObjIndicator != null)
            {
                missionObjIndicator.MarkObjectAsCompleted();

            }
            if (eliminatedTargetCount >= targetEnemies.Count)
            {
                Debug.Log("모든 타겟 제거 완료!");
                CheckMissionCompletion();
            }
        }
    }

    private void UpdateRemainingEnemiesUI()
    {
        int remainingEnemies = targetEnemies.Count - eliminatedTargetCount;

        // 이벤트를 통해 UI에 남은 적 수 업데이트
        if (RemainingEnemiesUpdated != null)
        {
            RemainingEnemiesUpdated.Invoke(remainingEnemies);
        }
    }

    public void OnMissionCompleted()
    {
        // 상호작용 미션 완료 처리
        currentMissionIndex++;
        if (currentMissionIndex < missionList.Count)
        {
            ActivateMission(currentMissionIndex);
        }
        else
        {
            Debug.Log("모든 상호작용 미션 완료!");
            CheckMissionCompletion();
        }
    }

    public void CheckMissionCompletion()
    {
        // 타겟 적 처치와 상호작용 미션이 모두 클리어되었는지 확인
        if (eliminatedTargetCount >= targetEnemies.Count && currentMissionIndex >= missionList.Count)
        {
            Debug.Log("모든 미션 완료! ClearInteractiveObj 활성화!");
            ActivateClearInteractive();
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
