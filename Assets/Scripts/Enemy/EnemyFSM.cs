using UnityEngine;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
    public IEnemyState CurrentState { get; private set; }
    private NavMeshAgent agent;

    public float FindDistance { get; set; } = 8f;
    public float AttackDistance { get; set; } = 2f;
    public float MoveSpeed { get; set; } = 5f;
    public int ATK { get; set; } = 5;
    public int HP { get; set; } = 100;
    public float AttackDelay { get; set; } = 2f;
    public float CurrentTime { get; set; } = 0f;

    private Transform player;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            agent = gameObject.AddComponent<NavMeshAgent>();
        }
        agent.enabled = true;
        player = GameObject.Find("Player").transform;
        SetState(new EnemyIdle(this));
    }

    void LateUpdate()
    {
        if (CurrentState != null)
        {
            CurrentTime += Time.deltaTime;
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
        return CurrentTime >= AttackDelay;
    }

    public void ResetAttackTime()
    {
        CurrentTime = 0f;
    }

    public Transform GetPlayer()
    {
        return player;
    }

    public void DisableNavMesh()
    {
        agent.enabled = false;
    }
}
