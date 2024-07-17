using UnityEngine;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
    public IEnemyState CurrentState { get; private set; }
    private NavMeshAgent agent;
    private Transform player;
    private Animator ani;

    public EnemyAttributes att = new EnemyAttributes();

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            agent = gameObject.AddComponent<NavMeshAgent>();
        }
        agent.speed = att.MoveSpeed;
        agent.enabled = true;
        ani = GetComponent<Animator>();

        player = GameObject.Find("Player").transform;
        
        SetState(new EnemyIdle(this));
    }

    void LateUpdate()
    {
        if (CurrentState != null)
        {
            att.CurrentTime += Time.deltaTime;
            CurrentState.Execute();
        }
    }

    public void SetState(IEnemyState newState)
    {
        if (CurrentState != null)
        {
            CurrentState.Exit();
        }
        CurrentState = newState;
        CurrentState.Enter();
    }

    public void TakeDamage(int damage)
    {
        CurrentState.TakeDamage(damage);
    }

    public void MoveTo(Vector3 destination)
    {
        if (agent.enabled)
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
        if (agent.enabled)
        {
            agent.isStopped = false;
        }
    }

    public bool CanAttack()
    {
        return att.CurrentTime >= att.AttackDelay;
    }

    public void ResetAttackTime()
    {
        att.CurrentTime = 0f;
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
