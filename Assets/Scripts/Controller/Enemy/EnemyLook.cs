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
        Collider[] temp = Physics.OverlapSphere(this.transform.position, _status.searchRadius, LayerMask.GetMask("Player", "Trap"));
        RaycastHit hit;
        
        distanceToPlayer = Mathf.Infinity;
        _gizmoColor = temp.Length > 0;

        if (temp.Length == 0)
            return false;

        //범위내에 여러 방해 오브젝트가 있을 수 있으므로 동적배열로 선언
        List<IinteractableObj> interactiveObjArray = new List<IinteractableObj>();

        foreach (Collider item in temp)
        {
            IinteractableObj objTemp = item.GetComponent<IinteractableObj>();
            if (objTemp != null)
            {
                interactiveObjArray.Add(objTemp);
                continue; //Player면서 상호작용오브제일 수는 없으므로 null 아니면 바로 break
            }

            //아래부터는 obj가 아니라는 것이 확실하므로
            _status.player = item.transform;
        }

        //조건을 만족하면 알아서 바뀜
        _status.attraction = false;
        _status.trapTransform = null;
        foreach (IinteractableObj item in interactiveObjArray)
        {
            //만약 배열이 없다면 실행되지 않을 것
            if (item.calling)
            {
                _status.attraction = true;
                _status.trapTransform = ((MonoBehaviour)item).transform;
            }
        }

        //플레이어가 범위내에 없으면 못찾으니까 false 반환
        if(_status.player == null) { return false; }
        //간단한 방향 벡터 구하기
        Vector3 directionToTarget = (_status.player.position - this.transform.position).normalized;

        //내적해서 각도 계산
        if (Vector3.Dot(directionToTarget, this.transform.forward) < 0.4f)
        {
            //만약 조건 미달이면 플레이어가 뒤쯤 있다는 것이므로 처형가능
            _status.executable = true;
            return false;
        }
        else
        {
            _status.executable = false;
        }
        //내적값 통과하면 Raycast해서 사이에 별도의 오브젝트 있는지 체크
        if (Physics.Raycast(this.transform.position + Vector3.up * 1.5f, directionToTarget, out hit, _status.searchRadius) == false)
            return false;

        //돌아야 하는지 체크
        CurvedCheck(directionToTarget);

        Debug.DrawLine(this.transform.position + Vector3.up * 1.5f, _status.player.position + Vector3.up * 1.5f);

        if (hit.transform.gameObject.layer != LayerMask.NameToLayer("Player"))
            return false; // 레이어가 Player가 아니면 false 반환

        distanceToPlayer = Vector3.Distance(this.transform.position, _status.player.position);

        //모든 조건을 통과했으므로 true 반환
        return true;
    }
    void StateUpdate()
    {
        //플레이어가 감지하고 상태를 바꾸는 메소드
        if (_status.IsAlive == false || _status.executing) { return; }

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
        if (_status.IsAlive == false || _status.executing) { return; }
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
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmoColor ? Color.green : Color.red;

        Gizmos.DrawWireSphere(this.transform.position, 10f);
    }
}
