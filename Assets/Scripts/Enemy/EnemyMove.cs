using UnityEngine;

public class EnemyMove : EnemyState
{
    public EnemyMove(EnemyFSM enemy) : base(enemy) { }

    public override void Enter()
    {
        // Move 상태 진입 시 초기화 작업
        Debug.Log("Entering Move State");
        enemy.MoveTo(enemy.GetPlayer().position); // NavMesh를 이용한 이동 시작
    }

    public override void Execute()
    {
        // Move 상태의 로직 처리
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
        // Move 상태 종료 시 정리 작업
        Debug.Log("Exiting Move State");
    }
}