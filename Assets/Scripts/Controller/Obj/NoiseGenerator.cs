using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGenerator : InteractableObjExtand
{
    AudioSource _audio;

    [Header("Sound")]
    [SerializeField] AudioClip Sound;
    [SerializeField] GameObject testGameobj;

    [Header("Setting")]
    [SerializeField] float _workingTime;
    float _timeDelta = 0f;

    [Header("UI")]
    [SerializeField] GameObject interactionUI;
    [SerializeField] Vector3 uiOffset = new Vector3(0, 1f, 0); // UI 위치 조정을 위한 오프셋

    static GameObject currentInteractingObject = null; // 현재 상호작용 중인 오브젝트

    private void Start()
    {
        _audio = this.GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (calling && _timeDelta < _workingTime)
            _timeDelta += Time.deltaTime;

        if (_timeDelta >= _workingTime)
            calling = false;

        UIShow();
        ui_Show = false;
    }

    void UIShow()
    {
        if (ui_Show == false)
        {
            if (currentInteractingObject == this.gameObject)
            {
                testGameobj.SetActive(false); // 테스트 obj

                // 현재 상호작용 중인 오브젝트와 일치하는 경우에만 UI 비활성화
                interactionUI.SetActive(false);
                currentInteractingObject = null;
            }
            return;
        }

        // 현재 상호작용 중인 오브젝트 설정
        currentInteractingObject = this.gameObject;

        testGameobj.SetActive(true); // 테스트 obj

        // UI 활성화 및 위치 업데이트
        UpdateUIPosition();
        interactionUI.SetActive(true);
    }
    public override void Interaction()
    {
        if(interactable == false) { return; }

        base.Interaction();
        if(Sound == null) { Debug.Log($"Sound of {this.gameObject.name} is null"); }

        interactable = false;
        calling = true;
    }

    // UI를 오브젝트 위치 근처로 이동
    void UpdateUIPosition()
    {
        if (interactionUI != null)
        {
            interactionUI.transform.position = transform.position + uiOffset;
        }
    }
}
