using UnityEngine;
using System.Collections;

public class EnemyDead : MonoBehaviour, IEnemyState
{
    private EnemyFSM enemyFSM;

    public void Enter(EnemyFSM enemy)
    {
        enemyFSM = enemy;
        Debug.Log("Entering Dead State");
        enemyFSM.isDead = true;
        enemyFSM.SetAnimatorParameter("IsDead", true);
        enemyFSM.StopMoving();
        enemyFSM.StartCoroutine(DieProcess());
    }

    public void Execute()
    {
        // Dead 상태에서 수행할 작업은 없습니다.
    }

    public void Exit()
    {
        Debug.Log("Exiting Dead State");
    }

    private IEnumerator DieProcess()
    {
        enemyFSM.DisableNavMesh(); // Disable NavMeshAgent
        yield return new WaitForSeconds(5f); // Wait before destroying the object
        Destroy(enemyFSM.gameObject);
    }
    public void TakeDamage(int damage)
    {
        // Attack 상태에서는 TakeDamage 메서드가 필요없을 수 있습니다.
        // 또는 필요한 경우 적절히 처리합니다.
    }
}
