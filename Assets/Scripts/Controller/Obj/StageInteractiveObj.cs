using UnityEngine;

public class StageInteractiveObj : InteractableObjExtand
{
    [Tooltip("true면 스테이지 클리어 조건 상호작용")]
    [SerializeField] private bool ClearCondition;

    private AudioSource _effectSound;

    private void Start()
    {
        if (ClearCondition)
        {
            GameManager.Stage.remainingStageObj++;
        }
    }

    public override void Interaction()
    {
        if (!interactable) return; // 상호작용 불가하면 리턴

        base.Interaction();

        if (_effectSound != null)
        {
            _effectSound.Play(); // 효과음 재생
        }

        Debug.Log($"{this.gameObject.name} Interaction");

        // ClearCondition이 true로 설정되었는지 확인
        if (!ClearCondition)
        {
            ClearCondition = true;  // ClearCondition을 true로 변경
            Debug.Log("ClearCondition is now true.");
        }

        if (ClearCondition)
        {
            GameManager.Stage.remainingStageObj--;
            FindObjectOfType<MissionManager>().ActivateNextMission(); // 다음 미션 활성화
        }

        interactable = false; // 한 번 상호작용 후 더 이상 상호작용 불가
    }

    // 상호작용 가능 여부 설정
    public void SetInteractable(bool value)
    {
        interactable = value;
        Debug.Log($"{this.gameObject.name} Interactable: {value}");
    }
}
