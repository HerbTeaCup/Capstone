using UnityEngine;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
    public IEnemyState CurrentState { get; private set; }
    private NavMeshAgent agent;
    private Transform player;
    private Animator ani;

    public EnemyAttributes att = new EnemyAttributes();
    public bool isDead = false;

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
        if (CurrentState != null && !isDead) // 현재 상태가 없거나 isDead 상태일 때 업데이트 중단
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
        if (!isDead) // isDead 상태일 때는 데미지 받지 않음
        {
            CurrentState.TakeDamage(damage);
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
