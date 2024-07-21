using UnityEngine;

public class EnemyIdle : MonoBehaviour, IEnemyState
{
    private EnemyFSM enemyFSM;

    public void Enter(EnemyFSM enemy)
    {
        enemyFSM = enemy;
        Debug.Log("Entering Idle State");
        enemy.SetAnimatorParameter("IsIdle", true);
    }

    public void Execute()
    {
        if (Vector3.Distance(enemyFSM.transform.position, enemyFSM.GetPlayer().position) < enemyFSM.GetComponent<EnemyStatus>().FindDistance)
        {
            enemyFSM.SetState(enemyFSM.moveState);
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Idle State");
        enemyFSM.SetAnimatorParameter("IsIdle", false);
    }
    public void TakeDamage(int damage)
    {
        // Attack ���¿����� TakeDamage �޼��尡 �ʿ���� �� �ֽ��ϴ�.
        // �Ǵ� �ʿ��� ��� ������ ó���մϴ�.
    }
}
