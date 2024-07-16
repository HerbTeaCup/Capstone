using UnityEngine;

public class EnemyMove : EnemyState
{
    public EnemyMove(EnemyFSM enemy) : base(enemy) { }

    public override void Enter()
    {
        // Move ���� ���� �� �ʱ�ȭ �۾�
        Debug.Log("Entering Move State");
        enemy.MoveTo(enemy.GetPlayer().position); // NavMesh�� �̿��� �̵� ����
    }

    public override void Execute()
    {
        // Move ������ ���� ó��
        if (Vector3.Distance(enemy.transform.position, enemy.GetPlayer().position) <= enemy.AttackDistance)
        {
            enemy.SetState(new EnemyAttack(enemy));
        }
        else
        {
            enemy.SetState(new EnemyMove(enemy));
        }
    }

    public override void Exit()
    {
        // Move ���� ���� �� ���� �۾�
        Debug.Log("Exiting Move State");
    }
}