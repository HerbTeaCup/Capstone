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
    [SerializeField] Vector3 uiOffset = new Vector3(0, 1f, 0); // UI ��ġ ������ ���� ������

    static GameObject currentInteractingObject = null; // ���� ��ȣ�ۿ� ���� ������Ʈ

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
                testGameobj.SetActive(false); // �׽�Ʈ obj

                // ���� ��ȣ�ۿ� ���� ������Ʈ�� ��ġ�ϴ� ��쿡�� UI ��Ȱ��ȭ
                interactionUI.SetActive(false);
                currentInteractingObject = null;
            }
            return;
        }

        // ���� ��ȣ�ۿ� ���� ������Ʈ ����
        currentInteractingObject = this.gameObject;

        testGameobj.SetActive(true); // �׽�Ʈ obj

        // UI Ȱ��ȭ �� ��ġ ������Ʈ
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

    // UI�� ������Ʈ ��ġ ��ó�� �̵�
    void UpdateUIPosition()
    {
        if (interactionUI != null)
        {
            interactionUI.transform.position = transform.position + uiOffset;
        }
    }
}
