using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    EnemyStatus _status;
    NavMeshAgent _navAgent;

    [SerializeField] Transform[] PatrolPoints;

    int _patrolIndex = 0;
    int _indexCashe = -1;

    // Start is called before the first frame update
    void Start()
    {
        _status = GetComponent<EnemyStatus>();
        _navAgent = GetComponent<NavMeshAgent>();

        GameManager.Enemy.UpdateDelegate += IdleMove;
        GameManager.Enemy.UpdateDelegate += BoundaryMove;
        GameManager.Enemy.UpdateDelegate += CatureMove;
        GameManager.Enemy.UpdateDelegate += Attraction;
    }

    void IdleMove()
    {
        if (_status.IsAlive == false || _status.executing) { _navAgent.isStopped = true; return; }
        if (_status.attraction) { return; }
        if (_status.state != EnemyState.Idle)
            return;

        if (PatrolPoints.Length == 0)
            return;

        _navAgent.isStopped = false;
        _navAgent.speed = Mathf.Lerp(_navAgent.speed, _status.walkSpeed, 10 * Time.deltaTime);

        if (_navAgent.remainingDistance - _navAgent.stoppingDistance < 0.1f)
        {
            _patrolIndex++;
            _patrolIndex = IndexClamping(_patrolIndex, 0, PatrolPoints.Length - 1);
        }

        if (_patrolIndex != _indexCashe)
        {
            _indexCashe = _patrolIndex;
            _navAgent.SetDestination(PatrolPoints[_patrolIndex].position);
        }

        _status.currnetSpeed = _navAgent.speed;
    }
    void BoundaryMove()
    {
        if (_status.IsAlive == false || _status.executing) { _navAgent.isStopped = true; return; }
        if (_status.state != EnemyState.Boundary)
            return;

        _navAgent.isStopped = true;
        this.transform.LookAt(_status.player.transform.position);
    }
    void CatureMove()
    {
        //if (_status.IsAlive == false || _status.executing) { _navAgent.isStopped = true; return; }
        //if (_status.state != EnemyState.Capture)
        //    return;

        //_navAgent.isStopped = false;

        //float targetSpeed = 0f;
        //if (_navAgent.remainingDistance < 5f)
        //{
        //    _navAgent.isStopped = true;
        //    targetSpeed = 0f;
        //    _status.currnetSpeed = 0f;
        //}
        //else
        //{
        //    _navAgent.isStopped = false;
        //    targetSpeed = _status.runSpeed;
        //}
        //if (_status.curveNeed)
        //{
        //    _navAgent.isStopped = false;
        //    targetSpeed = _status.runSpeed;
        //}

        //Debug.Log(_navAgent.remainingDistance);

        //_navAgent.speed = Mathf.Lerp(_navAgent.speed, targetSpeed, 10 * Time.deltaTime);
        //_navAgent.SetDestination(_status.player.transform.position);
        //this.transform.LookAt(_status.player.transform.position);

        //_status.currnetSpeed = _navAgent.speed;

        if (_status.IsAlive == false || _status.executing)
        {
            _navAgent.isStopped = true;
            return;
        }
        if (_status.state != EnemyState.Capture)
            return;

        _navAgent.isStopped = false;

        float targetSpeed = 0f;

        // 플레이어와의 거리가 5f 미만일 때
        if (_navAgent.remainingDistance < 5f)
        {
            if (_status.curveNeed)
            {
                _navAgent.isStopped = true;
                targetSpeed = _status.runSpeed;
            }
            else
            {
                _navAgent.isStopped = false;
                targetSpeed = 0f;
                _status.currnetSpeed = 0f;
            }
        }
        else
        {
            // 5f 이상일 때 플레이어 추적
            _navAgent.isStopped = false;
            targetSpeed = _status.runSpeed;
        }

        // 속도를 자연스럽게 변경
        _navAgent.speed = Mathf.Lerp(_navAgent.speed, targetSpeed, 10 * Time.deltaTime);

        // 목적지를 플레이어의 위치로 설정
        _navAgent.SetDestination(_status.player.transform.position);

        // 플레이어 쪽으로 회전
        this.transform.LookAt(_status.player.transform.position);

        // 현재 속도 업데이트
        _status.currnetSpeed = _navAgent.speed;
    }
    void Attraction()
    {
        if (_status.IsAlive == false || _status.executing) { _navAgent.isStopped = true; return; }
        if (_status.state == EnemyState.Boundary || _status.state == EnemyState.Capture)
            return;
        if (_status.attraction == false) { return; }
        if(_status.trapTransform == null) { Debug.Log($"trapTransform is null"); return; }

        _navAgent.SetDestination(_status.trapTransform.position);
        _navAgent.speed = Mathf.Lerp(_navAgent.speed, this._status.walkSpeed, 10 * Time.deltaTime);

        _status.currnetSpeed = _navAgent.speed;
    }

    int IndexClamping(int value, int min, int max)
    {
        if (value < min)
        {
            return max;
        }
        else if (value > max)
        {
            return min;
        }
        else
        {
            return value;
        }
    }
}
