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
        // Dead ���¿��� ������ �۾��� �����ϴ�.
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
        // Attack ���¿����� TakeDamage �޼��尡 �ʿ���� �� �ֽ��ϴ�.
        // �Ǵ� �ʿ��� ��� ������ ó���մϴ�.
    }
}
