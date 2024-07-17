using UnityEngine;

public class EnemyMove : EnemyState
{
    public EnemyMove(EnemyFSM enemy) : base(enemy) { }

    public override void Enter()
    {
        // Move 상태 진입 시 초기화 작업
        Debug.Log("Entering Move State");
        enemy.SetAnimatorParameter("IsMove", true);
        enemy.MoveTo(enemy.GetPlayer().position);
        enemy.ResumeMoving();
    }

    public override void Execute()
    {
        // Move 상태의 로직 처리
        if (Vector3.Distance(enemy.transform.position, enemy.GetPlayer().position) <= enemy.att.AttackDistance)
        {
            enemy.SetState(new EnemyAttack(enemy));
        }
        else
        {
            enemy.MoveTo(enemy.GetPlayer().position); // 계속해서 플레이어를 향해 이동
        }
    }

    public override void Exit()
    {
        // Move 상태 종료 시 정리 작업
        Debug.Log("Exiting Move State");
        enemy.SetAnimatorParameter("IsMove", false);
        enemy.StopMoving();
    }
}