using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInteractive : InteractableObjExtand
{
    EnemyStatus _status;

    [Header("UI")]
    [SerializeField] GameObject stealthUI;
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
        UpdateUIPosition();
        UIShow();
        ui_Show = false;
    }

    public override void Interaction()
    {
        if (_status.IsAlive == false || _status.executable == false)
            return;

        _status.Hp = -1;
        base.Interaction();
        ui_Show = false;

        _status.executing = true; // ó�����ϴ� ��

        // ���� �÷��̾�� �� ������Ʈ�� ��ġ�� ����� y�� ȸ���� ����
        Vector3 targetPosition = this.transform.position;
        targetPosition.y = _status.player.transform.position.y; // y �� ����

        // LookAt ��� y�ุ�� ����� ȸ�� ����
        _status.player.transform.LookAt(targetPosition);
        this.transform.rotation = Quaternion.LookRotation(_status.player.transform.forward);

        Destroy(this.GetComponent<CapsuleCollider>());
    }

    void UIShow()
    {
        if (ui_Show == false || _status.IsAlive == false)
        {
            stealthUI.SetActive(false);
            return;
        }

        Debug.Log($"UIShow Logic is Working");

        stealthUI.SetActive(true);
    }

    void UpdateUIPosition() // UI �÷��̾� �Ӹ� ���� �̵�
    {
        if (_status.player == null)
            return;

        if (stealthUI != null)
        {
            stealthUI.transform.position = _status.player.transform.position + uiOffset;
        }
    }
}
