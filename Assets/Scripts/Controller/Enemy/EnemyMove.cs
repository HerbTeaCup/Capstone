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
    }

    void IdleMove()
    {
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
        if (_status.state != EnemyState.Boundary)
            return;

        _navAgent.isStopped = true;
        this.transform.LookAt(GameManager.Player.transform.position);
    }
    void CatureMove()
    {
        if (_status.state != EnemyState.Capture)
            return;

        _navAgent.isStopped = false;
        _navAgent.speed = Mathf.Lerp(_navAgent.speed, _status.walkSpeed, 10 * Time.deltaTime);

        _navAgent.SetDestination(GameManager.Player.transform.position);
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
