using UnityEngine;

public class EnemyMove : EnemyState
{
    public EnemyMove(EnemyFSM enemy) : base(enemy) { }

    public override void Enter()
    {
        // Move ���� ���� �� �ʱ�ȭ �۾�
        Debug.Log("Entering Move State");
        enemy.SetAnimatorParameter("IsMove", true);
        enemy.MoveTo(enemy.GetPlayer().position);
        enemy.ResumeMoving();
    }

    public override void Execute()
    {
        // Move ������ ���� ó��
        if (Vector3.Distance(enemy.transform.position, enemy.GetPlayer().position) <= enemy.att.AttackDistance)
        {
            enemy.SetState(new EnemyAttack(enemy));
        }
        else
        {
            enemy.MoveTo(enemy.GetPlayer().position); // ����ؼ� �÷��̾ ���� �̵�
        }
    }

    public override void Exit()
    {
        // Move ���� ���� �� ���� �۾�
        Debug.Log("Exiting Move State");
        enemy.SetAnimatorParameter("IsMove", false);
        enemy.StopMoving();
    }
}