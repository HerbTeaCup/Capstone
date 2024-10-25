using UnityEngine;
using UnityEngine.UI;

public class RemainingEnemiesUI : MonoBehaviour
{
    [SerializeField] private Text remainingEnemiesText;

    private void Start()
    {
        // MissionManager의 이벤트를 구독하여 남은 적 수가 업데이트될 때마다 UI에 반영
        MissionManager missionManager = FindObjectOfType<MissionManager>();
        if (missionManager != null)
        {
            missionManager.RemainingEnemiesUpdated += UpdateRemainingEnemiesText;

            int initialCount = missionManager.targetEnemies.Count;
            UpdateRemainingEnemiesText(initialCount - missionManager.eliminatedTargetCount);
        }
    }

    private void UpdateRemainingEnemiesText(int remainingEnemies)
    {
        remainingEnemiesText.text = $"Remaining Enemies: {remainingEnemies}";
    }

    private void OnDestroy()
    {
        // 이벤트 구독 해제
        MissionManager missionManager = FindObjectOfType<MissionManager>();
        if (missionManager != null)
        {
            missionManager.RemainingEnemiesUpdated -= UpdateRemainingEnemiesText;
        }
    }
}
