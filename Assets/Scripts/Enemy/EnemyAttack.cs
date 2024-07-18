using System.Collections;
using UnityEngine;

public class EnemyAttack : EnemyState
{
    public EnemyAttack(EnemyFSM enemy) : base(enemy) { }

    public override void Enter()
    {
        // Attack 상태 진입 시 초기화 작업
        Debug.Log("Entering Attack State");
        enemy.SetAnimatorParameter("IsAttack", true);
        enemy.StopMoving();
    }

    public override void Execute()
    {
        // Attack 상태의 로직 처리
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
        // Attack 상태 종료 시 정리 작업
        Debug.Log("Exiting Attack State");
        enemy.SetAnimatorParameter("IsAttack", false);
    }
}
