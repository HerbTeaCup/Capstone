using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageInteractiveObj : InteractableObjExtand
{
    AudioSource _effecSound;

    private void Start()
    {
        GameManager.Stage.remainingStageObj++;
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

        GameManager.Stage.remainingStageObj--;
        interactable = false;
    }
}
