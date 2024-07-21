using UnityEngine;

public class EnemyAttack : MonoBehaviour, IEnemyState
{
    private EnemyFSM enemyFSM;

    public void Enter(EnemyFSM enemy)
    {
        enemyFSM = enemy;
        Debug.Log("Entering Attack State");
        enemyFSM.SetAnimatorParameter("IsAttack", true);
        enemyFSM.StopMoving();
    }

    public void Execute()
    {
        if (!enemyFSM.isDead && Vector3.Distance(enemyFSM.transform.position, enemyFSM.GetPlayer().position) <= enemyFSM.GetComponent<EnemyStatus>().AttackDistance)
        {
            if (enemyFSM.CanAttack())
            {
                // enemyFSM.GetPlayer().GetComponent<PlayerStatus>().TakeDamage(enemyFSM.GetComponent<EnemyStatus>().ATK);
                enemyFSM.GetPlayer().GetComponent<PlayerMove>().TakeDamage(enemyFSM.GetComponent<EnemyStatus>().ATK);
                enemyFSM.ResetAttackTime();
            }
        }
        else
        {
            enemyFSM.SetState(enemyFSM.moveState);
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Attack State");
        enemyFSM.SetAnimatorParameter("IsAttack", false);
    }

    public void TakeDamage(int damage)
    {
        // Attack 상태에서는 TakeDamage 메서드가 필요없을 수 있습니다.
        // 또는 필요한 경우 적절히 처리합니다.
    }
}
