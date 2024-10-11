using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyInteractive : InteractableObjExtand
{
    EnemyStatus _status;

    

    [Header("Detection UI")]
    [SerializeField] GameObject weakDetectionUI; // '?', �÷��̾� �߰�
    [SerializeField] GameObject strongDetectionUI; // '!', �÷��̾� ������ Ž��

    [Header("UI Settings")]
    [SerializeField] Vector3 uiOffset = new Vector3(0, 2f, 0); // �Ӹ� ���� UI�� �ø��� ���� ������ ��

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

        _status.executing = true; // ó�����ϴ� ��

        // ���� �÷��̾�� �� ������Ʈ�� ��ġ�� ����� y�� ȸ���� ����
        Vector3 targetPosition = this.transform.position;
        targetPosition.y = _status.player.transform.position.y; // y �� ����

        // LookAt ��� y�ุ�� ����� ȸ�� ����
        _status.player.transform.LookAt(targetPosition);
        this.transform.rotation = Quaternion.LookRotation(_status.player.transform.forward);
    }

    void UIShow(GameObject obj)
    {
        if (obj != null && !obj.activeSelf)
        {
            obj.SetActive(true);  // UI�� ���� ������ �ѱ�
            Debug.Log($"UI {obj.name} activated");
        }

    }
    void UIHide(GameObject obj)
    {
        if (obj != null && obj.activeSelf)
        {
            obj.SetActive(false);  // UI�� ���� ������ ����
            Debug.Log($"UI {obj.name} deactivated");
        }

    }

    void UpdateUIPosition(GameObject obj) // UI ���� �Ӹ� ���� �̵�
    {
        if (obj != null)
        {
            obj.transform.position = _status.transform.position + uiOffset;
        }
    }
}
