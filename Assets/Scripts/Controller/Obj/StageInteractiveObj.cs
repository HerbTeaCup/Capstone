using UnityEngine;

public class StageInteractiveObj : InteractableObjExtand
{
    [Tooltip("true�� �������� Ŭ���� ���� ��ȣ�ۿ�")]
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
        if (!interactable) return; // ��ȣ�ۿ� �Ұ��ϸ� ����

        base.Interaction();

        if (_effectSound != null)
        {
            _effectSound.Play(); // ȿ���� ���
        }

        Debug.Log($"{this.gameObject.name} Interaction");

        // ClearCondition�� true�� �����Ǿ����� Ȯ��
        if (!ClearCondition)
        {
            ClearCondition = true;  // ClearCondition�� true�� ����
            Debug.Log("ClearCondition is now true.");
        }

        if (ClearCondition)
        {
            GameManager.Stage.remainingStageObj--;
            FindObjectOfType<MissionManager>().ActivateNextMission(); // ���� �̼� Ȱ��ȭ
        }

        interactable = false; // �� �� ��ȣ�ۿ� �� �� �̻� ��ȣ�ۿ� �Ұ�
    }

    // ��ȣ�ۿ� ���� ���� ����
    public void SetInteractable(bool value)
    {
        interactable = value;
        Debug.Log($"{this.gameObject.name} Interactable: {value}");
    }
}
