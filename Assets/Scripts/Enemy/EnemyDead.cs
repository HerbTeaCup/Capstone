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
        // Die ������ �߰� ���� ó��
    }

    public override void Exit()
    {
        Debug.Log("Exiting Die State");
    }

    private IEnumerator DieProcess()
    {
        // ���� ���� ó��
        DisableNavMesh(); // NavMeshAgent ��Ȱ��ȭ
        yield return new WaitForSeconds(2f); // 2�� �� �� ��ü ����
        GameObject.Destroy(enemy.gameObject);
    }

    private void DisableNavMesh()
    {
        if (agent != null)
        {
            agent.enabled = false; // NavMeshAgent ��Ȱ��ȭ
        }
    }
}
