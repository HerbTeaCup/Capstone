using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInteractiveObj : InteractableObjExtand
{
    [Tooltip("true면 스테이지 클리어 조건 상호작용")] [SerializeField] bool ClearCondition;

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
            //효과음 있으면 재생
        }

        Debug.Log($"{this.gameObject.name} Interaction");

        if (ClearCondition)
        {
            GameManager.Stage.remainingStageObj--;
        }
        interactable = false;
    }
}
