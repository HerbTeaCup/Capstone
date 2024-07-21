using UnityEngine;

public class EnemyMove : MonoBehaviour, IEnemyState
{
    private EnemyFSM enemyFSM;

    public void Enter(EnemyFSM enemy)
    {
        enemyFSM = enemy;
        Debug.Log("Entering Move State");
        enemyFSM.SetAnimatorParameter("IsMove", true);
        enemyFSM.MoveTo(enemyFSM.GetPlayer().position);
        enemyFSM.ResumeMoving();
    }

    public void Execute()
    {
        if (Vector3.Distance(enemyFSM.transform.position, enemyFSM.GetPlayer().position) <= enemyFSM.GetComponent<EnemyStatus>().AttackDistance)
        {
            enemyFSM.SetState(enemyFSM.attackState);
        }
        else
        {
            enemyFSM.MoveTo(enemyFSM.GetPlayer().position);
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Move State");
        enemyFSM.SetAnimatorParameter("IsMove", false);
        enemyFSM.StopMoving();
    }
    public void TakeDamage(int damage)
    {
        // Attack ���¿����� TakeDamage �޼��尡 �ʿ���� �� �ֽ��ϴ�.
        // �Ǵ� �ʿ��� ��� ������ ó���մϴ�.
    }
}
