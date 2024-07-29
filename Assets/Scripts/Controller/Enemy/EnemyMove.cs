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

        if (PatrolPoints.Length > 0)
            return;

        if (_navAgent.remainingDistance < 0.2f)
        {
            _patrolIndex++;
            _patrolIndex = Mathf.Clamp(_patrolIndex, 0, PatrolPoints.Length);
        }

    }
    void BoundaryMove()
    {
        if (_status.state != EnemyState.Boundary)
            return;
    }
    void CatureMove()
    {
        if (_status.state != EnemyState.Capture)
            return;
    }

    int IndexClamping(int value, int min, int max)
    {
        if (value < min)
        {
            return max;
        }
        else if (value >= max)
        {
            return min;
        }
        else
        {
            return value;
        }
    }
}
