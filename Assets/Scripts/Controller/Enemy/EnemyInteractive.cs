using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInteractive : InteractableObjExtand
{
    EnemyStatus _status;

    [Header("UI")]
    [SerializeField] GameObject stealthUI;
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
        if (_status.executable == true && _status.executing == false)
        {
            UpdateUIPosition();
            UIShow();
        }
        ui_Show = false;
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

    void UIShow()
    {
        if (ui_Show == false)
        {
            stealthUI.SetActive(false);
            return;
        }

        Debug.Log($"UIShow Logic is Working");

        stealthUI.SetActive(true);
    }

    void UpdateUIPosition() // UI 플레이어 머리 위로 이동
    {
        if (stealthUI != null)
        {
            stealthUI.transform.position = _status.player.transform.position + uiOffset;
        }
    }
}
