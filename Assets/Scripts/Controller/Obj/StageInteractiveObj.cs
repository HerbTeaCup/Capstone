using UnityEngine;

public class StageInteractiveObj : InteractableObjExtand
{
    [Tooltip("true면 스테이지 클리어 조건 상호작용")]
    [SerializeField] private bool ClearCondition;

    private AudioSource _effectSound;
    private AudioClip _clip;
    private MissionObjIndicator missionObjIndicator;

    private void Start()
    {
        missionObjIndicator = FindObjectOfType<MissionObjIndicator>();
        _clip = Resources.Load<AudioClip>("Sound/correct-2-46134");

        if (ClearCondition)
        {
            GameManager.Stage.remainingStageObj++;
        }
    }

    public override void Interaction()
    {
        if (!interactable) return;

        base.Interaction();

        if (_effectSound != null)
        {
            _effectSound.PlayOneShot(_clip);
        }

        Debug.Log($"{this.gameObject.name} Interaction");

        if (!ClearCondition)
        {
            ClearCondition = true;
            Debug.Log("ClearCondition is now true.");

            if (missionObjIndicator != null)
            {
                missionObjIndicator.MarkObjectAsCompleted();
            }
        }

        if (ClearCondition)
        {
            GameManager.Stage.remainingStageObj--;
            FindObjectOfType<MissionManager>().ActivateNextMission();
        }

        interactable = false;
    }

    public void SetInteractable(bool value)
    {
        interactable = value;
        // Debug.Log($"{this.gameObject.name} Interactable: {value}");
    }
}
