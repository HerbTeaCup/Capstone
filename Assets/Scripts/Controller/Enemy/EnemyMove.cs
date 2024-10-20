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

        // �÷��̾���� �Ÿ��� 5f �̸��� ��
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
            // 5f �̻��� �� �÷��̾� ����
            _navAgent.isStopped = false;
            targetSpeed = _status.runSpeed;
        }

        // �ӵ��� �ڿ������� ����
        _navAgent.speed = Mathf.Lerp(_navAgent.speed, targetSpeed, 10 * Time.deltaTime);

        // �������� �÷��̾��� ��ġ�� ����
        _navAgent.SetDestination(_status.player.transform.position);

        // �÷��̾� ������ ȸ��
        this.transform.LookAt(_status.player.transform.position);

        // ���� �ӵ� ������Ʈ
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
