using System.Collections;
using UnityEngine;

public class EnemyDamaged : EnemyState
{
    public EnemyDamaged(EnemyFSM enemy) : base(enemy) { }

    public override void Enter()
    {
        Debug.Log("Entering Damaged State");
        // �ǰ� ��� �ð���ŭ ���
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
    }


    private IEnumerator DamageProcess()
    {
        Debug.Log("��� ����ߴٰ� �̵�");

        // �ǰ� ��� �ð���ŭ ���
        Debug.Log("Starting DamageProcess coroutine");
        yield return new WaitForSeconds(0.5f);
        Debug.Log("After WaitForSeconds in DamageProcess");

        // Move ���·� ��ȯ
        enemy.SetState(new EnemyMove(enemy));
    }
}