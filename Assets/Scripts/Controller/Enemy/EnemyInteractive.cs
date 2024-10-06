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

        // 현재 플레이어와 이 오브젝트의 위치를 사용해 y축 회전만 적용
        Vector3 targetPosition = this.transform.position;
        targetPosition.y = _status.player.transform.position.y; // y 축 고정

        // LookAt 대신 y축만을 고려한 회전 적용
        _status.player.transform.LookAt(targetPosition);

        this.transform.rotation = Quaternion.LookRotation(_status.player.transform.forward);
    }
}
