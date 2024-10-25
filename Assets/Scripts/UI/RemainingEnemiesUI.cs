using UnityEngine;
using UnityEngine.UI;

public class RemainingEnemiesUI : MonoBehaviour
{
    [SerializeField] private Text remainingEnemiesText;

    private void Start()
    {
        // MissionManager�� �̺�Ʈ�� �����Ͽ� ���� �� ���� ������Ʈ�� ������ UI�� �ݿ�
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
        // �̺�Ʈ ���� ����
        MissionManager missionManager = FindObjectOfType<MissionManager>();
        if (missionManager != null)
        {
            missionManager.RemainingEnemiesUpdated -= UpdateRemainingEnemiesText;
        }
    }
}
