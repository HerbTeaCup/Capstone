using UnityEngine.AI;
using UnityEngine;

public class EnemyFSM : MonoBehaviour, IEnemyDamageable
{
    public EnemyState currentState { get; private set; }
    private NavMeshAgent agent;
    private Transform player;
    private Animator ani;

    public EnemyStatus status;
    public EnemyIdle idleState;
    public EnemyMove moveState;
    public EnemyAttack attackState;
    public EnemyDamaged damagedState;
    public EnemyDead deadState;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            agent = gameObject.AddComponent<NavMeshAgent>();
        }
        agent.speed = status.MoveSpeed;
        agent.enabled = true;
        ani = GetComponent<Animator>();

        player = GameObject.Find("Player").transform;

        idleState = new EnemyIdle(this);
        moveState = new EnemyMove(this);
        attackState = new EnemyAttack(this);
        damagedState = new EnemyDamaged(this);
        deadState = new EnemyDead(this);

        TransitionToState(idleState);
    }

    void LateUpdate()
    {
        if (currentState != null && !status.isDead)
        {
            currentState.Execute();
        }
    }

    public void TransitionToState(EnemyState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter();
    }

    public void TakeDamage(int damage)
    {
        if (!status.isDead)
        {
            currentState.TakeDamage(damage);
        }
    }

    public void MoveTo(Vector3 destination)
    {
        if (agent.enabled && !status.isDead)
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
        if (agent.enabled && !status.isDead)
        {
            agent.isStopped = false;
        }
    }

    public bool CanAttack()
    {
        return status.AttackDelay <= 0;
    }

    public void ResetAttackTime()
    {
        status.AttackDelay = status.initialAttackDelay;
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
