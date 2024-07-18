using System.Collections;
using UnityEngine;

public class EnemyAttack : EnemyState
{
    public EnemyAttack(EnemyFSM enemy) : base(enemy) { }

    public override void Enter()
    {
        // Attack ���� ���� �� �ʱ�ȭ �۾�
        Debug.Log("Entering Attack State");
        enemy.SetAnimatorParameter("IsAttack", true);
        enemy.StopMoving();
    }

    public override void Execute()
    {
        // Attack ������ ���� ó��
        if (!enemy.isDead && Vector3.Distance(enemy.transform.position, enemy.GetPlayer().position) <= enemy.att.AttackDistance)
        {
            if (enemy.CanAttack())
            {
                enemy.GetPlayer().GetComponent<PlayerMove>().TakeDamage(enemy.att.ATK);
                enemy.ResetAttackTime();
            }
        }
        else
        {
            enemy.SetState(new EnemyMove(enemy));
        }
    }

    public override void Exit()
    {
        // Attack ���� ���� �� ���� �۾�
        Debug.Log("Exiting Attack State");
        enemy.SetAnimatorParameter("IsAttack", false);
    }
}
