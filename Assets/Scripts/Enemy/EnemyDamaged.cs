using System.Collections;
using UnityEngine;

public class EnemyDamaged : EnemyState
{
    public EnemyDamaged(EnemyFSM enemy) : base(enemy) { }

    public override void Enter()
    {
        Debug.Log("Entering Damaged State");
        // 피격 모션 시간만큼 대기
        enemy.SetAnimatorParameter("IsDamaged", true);
        enemy.StopMoving();
        enemy.StartCoroutine(DamageProcess());
    }

    public override void Execute()
    {
        // Damaged 상태의 로직 처리 (필요시 추가)
    }

    public override void Exit()
    {
        // Damaged 상태 종료 시 정리 작업
        Debug.Log("Exiting Damaged State");
        enemy.SetAnimatorParameter("IsDamaged", false);
        enemy.ResumeMoving();
    }


    private IEnumerator DamageProcess()
    {
        Debug.Log("잠시 대기했다가 이동");

        // 피격 모션 시간만큼 대기
        Debug.Log("Starting DamageProcess coroutine");
        yield return new WaitForSeconds(1.6f);
        Debug.Log("After WaitForSeconds in DamageProcess");

        // Move 상태로 전환
        enemy.SetState(new EnemyMove(enemy));
    }
}