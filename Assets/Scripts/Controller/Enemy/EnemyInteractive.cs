using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInteractive : InteractableObjExtand
{
    EnemyStatus _status;

    // Start is called before the first frame update
    void Start()
    {
        _status = GetComponent<EnemyStatus>();
    }
    private void Update()
    {
        //if(_status.player == null) { return; }
        //Debug.Log(_status.player.transform.position);
    }

    public override void Interaction()
    {
        if (_status.IsAlive == false || _status.executable == false)
            return;

        base.Interaction();

        _status.executing = true;

        // ���� �÷��̾�� �� ������Ʈ�� ��ġ�� ����� y�� ȸ���� ����
        Vector3 targetPosition = this.transform.position;
        targetPosition.y = _status.player.transform.position.y; // y �� ����

        // LookAt ��� y�ุ�� ����� ȸ�� ����
        _status.player.transform.LookAt(targetPosition);

        this.transform.rotation = Quaternion.LookRotation(_status.player.transform.forward);
    }
}
