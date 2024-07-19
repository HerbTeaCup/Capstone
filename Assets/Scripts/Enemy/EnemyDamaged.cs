using System.Collections;
using UnityEngine;

public class EnemyDamaged : EnemyState
{
    public EnemyDamaged(EnemyFSM enemy) : base(enemy) { }

    // �˹� �Ÿ�
    private float knockbackDistance = 2f;
    private Vector3 knockbackDirection;

    public override void Enter()
    {
        Debug.Log("Entering Damaged State");
        // �ǰ� ��� �ð���ŭ ���
        enemy.SetAnimatorParameter("IsDamaged", true);
        enemy.StopMoving();

        // �÷��̾�κ��� �з����� ���� ���
        knockbackDirection = (enemy.transform.position - enemy.GetPlayer().position).normalized * knockbackDistance;

        // ȿ�� ����
        enemy.StartCoroutine(DamageProcess());
    }

    public override void Execute()
    {
        // Damaged ������ ���� ó�� (�ʿ�� �߰�)
    }

    public override void Exit()
    {
        // Damaged ���� ���� �� ���� �۾�
        Debug.Log("Exiting Damaged State");
        enemy.SetAnimatorParameter("IsDamaged", false);
        enemy.ResumeMoving();
    }


    private IEnumerator DamageProcess()
    {
        Debug.Log("��� ����ߴٰ� �̵�");

        // �ǰ� ��� �ð���ŭ ���
        Debug.Log("Starting DamageProcess coroutine");

        // �˹� �ð� �� �˹� ����
        float knockbackTime = 0.2f;
        float elapsedTime = 0f;
        while (elapsedTime < knockbackTime)
        {
            enemy.transform.position += knockbackDirection * Time.deltaTime / knockbackTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        yield return new WaitForSeconds(1.3f); // 1.3f ����
        Debug.Log("After WaitForSeconds in DamageProcess");

        // Move ���·� ��ȯ
        enemy.SetState(new EnemyMove(enemy));
    }
}