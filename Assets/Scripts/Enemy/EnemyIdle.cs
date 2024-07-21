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
        // Attack 상태에서는 TakeDamage 메서드가 필요없을 수 있습니다.
        // 또는 필요한 경우 적절히 처리합니다.
    }
}
