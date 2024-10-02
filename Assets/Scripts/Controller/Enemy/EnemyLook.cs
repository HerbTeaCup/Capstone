using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLook : MonoBehaviour
{
    EnemyStatus _status;

    bool _gizmoColor = false;

    float _timeDelta = 0f;
    [SerializeField] float detectionRate;

    // Start is called before the first frame update
    void Start()
    {
        _status = GetComponent<EnemyStatus>();

        GameManager.Enemy.UpdateDelegate += StateUpdate;
    }

    bool Searching(out float distanceToPlayer)
    {
        Collider[] temp = Physics.OverlapSphere(this.transform.position, _status.searchRadius, LayerMask.GetMask("Player"));
        RaycastHit hit;
        
        distanceToPlayer = Mathf.Infinity;
        _gizmoColor = temp.Length > 0;

        if (temp.Length == 0)
            return false;

        Vector3 directionToTarget = (temp[0].transform.position - this.transform.position).normalized;

        if (Vector3.Dot(directionToTarget, this.transform.forward) < 0.4f)
            return false;
        if (Physics.Raycast(this.transform.position + Vector3.up * 1.5f, directionToTarget, out hit, _status.searchRadius) == false)
            return false;
        CurvedCheck(directionToTarget);

        Debug.DrawLine(this.transform.position + Vector3.up * 1.5f, temp[0].transform.position + Vector3.up * 1.5f);

        if (hit.transform.gameObject.layer != LayerMask.NameToLayer("Player"))
            return false; // 레이어가 Player가 아니면 false 반환

        distanceToPlayer = Vector3.Distance(this.transform.position, temp[0].transform.position);
        return true;
    }
    void StateUpdate()
    {
        //플레이어가 감지하고 상태를 바꾸는 메소드
        float distanceToPlayer;
        bool foundPlayer = Searching(out distanceToPlayer);
        
        _status.currentTime = _timeDelta;
        if (foundPlayer)
        {
            // 거리가 가까울수록 탐지 속도 증가
            float detectionSpeed = _status.searchRadius / Mathf.Max(distanceToPlayer, 0.1f) * detectionRate;
            _timeDelta += Time.deltaTime * detectionSpeed;

            //발각 상태가 되면 10초가 되서 5초의 유예시간을 줌
            if (_timeDelta > 10f || _status.state == EnemyState.Capture) { _timeDelta = 10f; return; }
        }
        else
        {
            _timeDelta = Mathf.Max(0, _timeDelta - Time.deltaTime);
            if(_timeDelta < 0.1f) { _timeDelta = 0f; }
        }

        if (_timeDelta > _status.captureTime)
        {
            _status.state = EnemyState.Capture;
        }
        else if (_timeDelta > _status.boundaryTime)
        {
            _status.state = EnemyState.Boundary;
        }
        else
        {
            _status.state = EnemyState.Idle;
        }
    }
    void CurvedCheck(Vector3 dir)
    {
        if (_status.state != EnemyState.Capture)
            return;

        if (Physics.Raycast(this.transform.position + Vector3.up, dir, _status.searchRadius, 1 << 8))
        {
            _status.curveNeed = true;
        }
        else
        {
            _status.curveNeed = false;
        }
        Debug.Log($"Raycast =  {_status.curveNeed}");
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmoColor ? Color.green : Color.red;

        Gizmos.DrawWireSphere(this.transform.position, 10f);
    }
}
