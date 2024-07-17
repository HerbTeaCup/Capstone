using System.Collections;
using UnityEngine;

public class EnemyIdle : EnemyState
{
    public EnemyIdle(EnemyFSM enemy) : base(enemy) { }

    public override void Enter()
    {
        // Idle 상태 진입 시 초기화 작업
        Debug.Log("Entering Idle State");
        enemy.SetAnimatorParameter("IsIdle", true);
    }

    public override void Execute()
    {
        // Idle 상태의 로직 처리
        if (Vector3.Distance(enemy.transform.position, enemy.GetPlayer().position) < enemy.att.FindDistance)
        {
            enemy.SetState(new EnemyMove(enemy));
        }
    }

    public override void Exit()
    {
        // Idle 상태 종료 시 정리 작업
        Debug.Log("Exiting Idle State");
        enemy.SetAnimatorParameter("IsIdle", false);
    }

}
