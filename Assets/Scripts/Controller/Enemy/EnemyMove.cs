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
        _navAgent.speed = Mathf.Lerp(_navAgent.speed, _status.runSpeed, 10 * Time.deltaTime);

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
    }
    void BoundaryMove()
    {
        if (_status.IsAlive == false || _status.executing) { _navAgent.isStopped = true; return; }
        if (_status.state != EnemyState.Boundary)
            return;

        _navAgent.isStopped = false;
        this.transform.LookAt(_status.player.transform.position);
    }
    void CatureMove()
    {
        if (_status.IsAlive == false || _status.executing) { _navAgent.isStopped = true; return; }
        if (_status.state != EnemyState.Capture)
            return;

        float targetSpeed = 0f;
        if (_navAgent.remainingDistance < 5f)
        {
            targetSpeed = 0f;
        }
        else
        {
            targetSpeed = 4f;
        }
        if (_status.curveNeed)
        {
            targetSpeed = 4f;
        }

        _navAgent.speed = Mathf.Lerp(_navAgent.speed, targetSpeed, 10 * Time.deltaTime);
        _navAgent.SetDestination(_status.player.transform.position);
        this.transform.LookAt(_status.player.transform.position);
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
