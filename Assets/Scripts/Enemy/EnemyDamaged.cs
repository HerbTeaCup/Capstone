using UnityEngine;
using System.Collections;

public class EnemyDamaged : MonoBehaviour, IEnemyState
{
    private EnemyFSM enemyFSM;
    private float knockbackDistance = 2f;
    private Vector3 knockbackDirection;

    public void Enter(EnemyFSM enemy)
    {
        enemyFSM = enemy;
        Debug.Log("Entering Damaged State");
        enemyFSM.SetAnimatorParameter("IsDamaged", true);
        enemyFSM.StopMoving();

        knockbackDirection = (enemyFSM.transform.position - enemyFSM.GetPlayer().position).normalized * knockbackDistance;

        enemyFSM.StartCoroutine(DamageProcess());
    }

    public void Execute()
    {
        if (enemyFSM.GetComponent<EnemyStatus>().isDead)
        {
            enemyFSM.SetState(enemyFSM.deadState);
        }
        else if (enemyFSM.GetComponent<EnemyStatus>().CurrentTime >= 1.3f)
        {
            enemyFSM.SetState(enemyFSM.idleState);
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Damaged State");
        enemyFSM.SetAnimatorParameter("IsDamaged", false);
        enemyFSM.ResumeMoving();
    }

    private IEnumerator DamageProcess()
    {
        float knockbackTime = 0.2f;
        float elapsedTime = 0f;
        while (elapsedTime < knockbackTime)
        {
            enemyFSM.transform.position += knockbackDirection * Time.deltaTime / knockbackTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1.3f); // Wait before transitioning to another state
        Debug.Log("After WaitForSeconds in DamageProcess");

        enemyFSM.SetState(enemyFSM.moveState);
    }

    public void TakeDamage(int damage)
    {
        // Attack ���¿����� TakeDamage �޼��尡 �ʿ���� �� �ֽ��ϴ�.
        // �Ǵ� �ʿ��� ��� ������ ó���մϴ�.
    }
}
