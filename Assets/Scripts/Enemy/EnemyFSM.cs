using UnityEngine;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
    public EnemyIdle idleState;
    public EnemyDead deadState;
    public EnemyDamaged damagedState;
    public EnemyAttack attackState;
    public EnemyMove moveState;

    private IEnemyState currentState;
    private EnemyStatus _status;
    private NavMeshAgent agent;
    private Transform player;
    private Animator ani;
    public bool isDead = false; // �߰��� �κ�

    private void Awake()
    {
        _status = GetComponent<EnemyStatus>();
        if (_status == null)
        {
            Debug.LogError("EnemyStatus component is not assigned!");
        }

        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            agent = gameObject.AddComponent<NavMeshAgent>();
        }

        ani = GetComponent<Animator>();
        player = GameObject.Find("Player").transform;

        idleState = gameObject.AddComponent<EnemyIdle>();
        deadState = gameObject.AddComponent<EnemyDead>();
        damagedState = gameObject.AddComponent<EnemyDamaged>();
        attackState = gameObject.AddComponent<EnemyAttack>();
        moveState = gameObject.AddComponent<EnemyMove>();

        SetState(idleState);
    }

    private void Update()
    {
        if (currentState != null && !isDead)
        {
            _status.CurrentTime += Time.deltaTime;
            currentState.Execute();
        }
    }

    public void SetState(IEnemyState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter(this);
    }

    public void TakeDamage(int damage)
    {
        if (!isDead)
        {
            currentState.TakeDamage(damage);
        }
    }

    public void MoveTo(Vector3 destination)
    {
        if (agent.enabled && !isDead)
        {
            agent.SetDestination(destination);
        }
    }

    public void StopMoving()
    {
        if (agent.enabled)
        {
            agent.isStopped = true;
        }
    }

    public void ResumeMoving()
    {
        if (agent.enabled && !isDead)
        {
            agent.isStopped = false;
        }
    }

    public bool CanAttack()
    {
        return _status.CurrentTime >= _status.AttackDelay;
    }

    public void ResetAttackTime()
    {
        _status.CurrentTime = 0f;
    }

    public Transform GetPlayer()
    {
        return player;
    }

    public void DisableNavMesh()
    {
        if (agent != null)
        {
            agent.enabled = false;
        }
    }

    public void SetAnimatorParameter(string parameter, bool value)
    {
        if (ani != null)
        {
            ani.SetBool(parameter, value);
        }
    }
}
