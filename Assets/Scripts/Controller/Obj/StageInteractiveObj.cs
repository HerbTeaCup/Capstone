using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInteractiveObj : InteractableObjExtand
{
    [Tooltip("true�� �������� Ŭ���� ���� ��ȣ�ۿ�")] [SerializeField] bool ClearCondition;

    AudioSource _effecSound;

    private void Start()
    {
        if (ClearCondition)
        {
            GameManager.Stage.remainingStageObj++;
        }
    }

    public override void Interaction()
    {
        base.Interaction();

        if (interactable == false)
            return;

        if (_effecSound != null)
        {
            //ȿ���� ������ ���
        }

        Debug.Log($"{this.gameObject.name} Interaction");

        if (ClearCondition)
        {
            GameManager.Stage.remainingStageObj--;
        }
        interactable = false;
    }
}
