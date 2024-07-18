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
        enemy.isDead = true; // ���� ���� ���·� ����
        enemy.SetAnimatorParameter("IsDead", true);
        enemy.StopMoving();
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
        enemy.DisableNavMesh(); // NavMeshAgent ��Ȱ��ȭ
        yield return new WaitForSeconds(5f); // 5�� �� �� ��ü ����
        GameObject.Destroy(enemy.gameObject);
    }
}
