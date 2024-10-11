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

    [Header("Stealth UI")]
    [SerializeField] GameObject stealthUI;

    [Header("UI Settings")]
    [SerializeField] Vector3 uiOffset = new Vector3(0, 2f, 0); // �Ӹ� ���� UI�� �ø��� ���� ������ ��
    [SerializeField] float stealthAngleThreshold = 60f; // �÷��̾ ���� �� �ڿ� �־�� �ϴ� ����
    [SerializeField] float maxStealthDistance = 2.5f; // �÷��̾�� �� ������ �ִ� ���ڽ� ��ȣ�ۿ� �Ÿ�

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
        if (_status.IsAlive == true && _status.executable == true && _status.executing == false && IsPlayerInStealthRange())
        {
            ui_Show = true;
            UpdateUIPosition(stealthUI);
            UIShow(stealthUI);
        }
        else
        {
            UIHide(stealthUI);
        }

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

        if (!ui_Show || _status.IsAlive == false)
        {
            UIHide(stealthUI);
            UIHide(weakDetectionUI);
            UIHide(strongDetectionUI);
        }
    }

    public override void Interaction()
    {
        if (_status.IsAlive == false || _status.executable == false || !IsPlayerInStealthRange())
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

    bool IsPlayerInStealthRange()
    {
        if (_status.player == null) return false;

        // 1. �÷��̾�� �� ������ �Ÿ� ���
        float distanceToPlayer = Vector3.Distance(_status.player.transform.position, transform.position);
        if (distanceToPlayer > maxStealthDistance) return false; // �Ÿ��� �ʹ� �ָ� false

        // 2. �÷��̾ ���� �ڿ� �ִ��� Ȯ��
        Vector3 toPlayer = _status.player.transform.position - transform.position; // ������ �÷��̾������ ����
        toPlayer.y = 0; // Y�� �� ���� (���� ���⸸ ���)

        Vector3 forward = transform.forward; // ���� ���� ����
        forward.y = 0; // Y�� �� ���� (���� ���⸸ ���)

        float angle = Vector3.Angle(forward, toPlayer); // �� ���� ������ ���� ���

        // �÷��̾ ���� �� �ڿ� �ִ��� Ȯ�� (stealthAngleThreshold ���� ���� ���)
        return angle > 180f - stealthAngleThreshold / 2f && angle < 180f + stealthAngleThreshold / 2f;
    }
}
