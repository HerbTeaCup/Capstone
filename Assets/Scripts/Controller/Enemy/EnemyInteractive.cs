using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyInteractive : InteractableObjExtand
{
    EnemyStatus _status;

    

    [Header("Detection UI")]
    [SerializeField] GameObject weakDetectionUI; // '?', 플레이어 발견
    [SerializeField] GameObject strongDetectionUI; // '!', 플레이어 완전히 탐지

    [Header("UI Settings")]
    [SerializeField] Vector3 uiOffset = new Vector3(0, 2f, 0); // 머리 위로 UI를 올리기 위한 오프셋 값

    // Start is called before the first frame update
    void Start()
    {
        _status = GetComponent<EnemyStatus>();
    }
    private void Update()
    {
        //if(_status.player == null) { return; }
        //Debug.Log(_status.player.transform.position);

        UpdateUI();
    }

    void UpdateUI()
    {

        if (_status.weakDetecting == true)
        {
            ui_Show = true;
            UpdateUIPosition(weakDetectionUI);
            UIShow(weakDetectionUI);
        }
        else
        {
            UIHide(weakDetectionUI);
        }
        
        if (_status.strongDetecting == true)
        {
            ui_Show = true;
            UpdateUIPosition(strongDetectionUI);
            UIShow(strongDetectionUI);
        }
        else
        {
            UIHide(strongDetectionUI);
        }

        if (!ui_Show)
        {
            UIHide(weakDetectionUI);
            UIHide(strongDetectionUI);
        }
    }

    public override void Interaction()
    {
        if (_status.IsAlive == false || _status.executable == false)
            return;
        
        base.Interaction();
        ui_Show = false;

        _status.executing = true; // 처형당하는 중

        // 현재 플레이어와 이 오브젝트의 위치를 사용해 y축 회전만 적용
        Vector3 targetPosition = this.transform.position;
        targetPosition.y = _status.player.transform.position.y; // y 축 고정

        // LookAt 대신 y축만을 고려한 회전 적용
        _status.player.transform.LookAt(targetPosition);
        this.transform.rotation = Quaternion.LookRotation(_status.player.transform.forward);
    }

    void UIShow(GameObject obj)
    {
        if (obj != null && !obj.activeSelf)
        {
            obj.SetActive(true);  // UI가 꺼져 있으면 켜기
            Debug.Log($"UI {obj.name} activated");
        }

    }
    void UIHide(GameObject obj)
    {
        if (obj != null && obj.activeSelf)
        {
            obj.SetActive(false);  // UI가 켜져 있으면 끄기
            Debug.Log($"UI {obj.name} deactivated");
        }

    }

    void UpdateUIPosition(GameObject obj) // UI 적의 머리 위로 이동
    {
        if (obj != null)
        {
            obj.transform.position = _status.transform.position + uiOffset;
        }
    }
}
