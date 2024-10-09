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
            //����
            testGameobj.SetActive(false);
            return;
        }

        //�ؿ������� �۾�����
        Debug.Log($"UIShow Logic is Working");
        //����
        testGameobj.SetActive(true);
    }
    public override void Interaction()
    {
        if(interactable == false) { return; }

        base.Interaction();
        if(Sound == null) { Debug.Log($"Sound of {this.gameObject.name} is null"); }

        interactable = false;
        calling = true;
    }
}
