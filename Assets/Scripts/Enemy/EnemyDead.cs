using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDead : EnemyState
{
    private NavMeshAgent agent;

    public EnemyDead(EnemyFSM enemy) : base(enemy)
    {
        agent = enemy.GetComponent<NavMeshAgent>();
    }

    public override void Enter()
    {
        Debug.Log("Entering Die State");
        enemy.StartCoroutine(DieProcess());
    }

    public override void Execute()
    {
        // Die 상태의 추가 로직 처리
    }

    public override void Exit()
    {
        Debug.Log("Exiting Die State");
    }

    private IEnumerator DieProcess()
    {
        // 죽음 상태 처리
        DisableNavMesh(); // NavMeshAgent 비활성화
        yield return new WaitForSeconds(2f); // 2초 후 적 객체 제거
        GameObject.Destroy(enemy.gameObject);
    }

    private void DisableNavMesh()
    {
        if (agent != null)
        {
            agent.enabled = false; // NavMeshAgent 비활성화
        }
    }
}
