using System.Collections;
using UnityEngine;

public class EnemyDamaged : EnemyState
{
    public EnemyDamaged(EnemyFSM enemy) : base(enemy) { }

    // 넉백 거리
    private float knockbackDistance = 2f;
    private Vector3 knockbackDirection;

    public override void Enter()
    {
        Debug.Log("Entering Damaged State");
        // 피격 모션 시간만큼 대기
        enemy.SetAnimatorParameter("IsDamaged", true);
        enemy.StopMoving();

        // 플레이어로부터 밀려나는 방향 계산
        knockbackDirection = (enemy.transform.position - enemy.GetPlayer().position).normalized * knockbackDistance;

        // 효과 적용
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

        // 넉백 시간 및 넉백 적용
        float knockbackTime = 0.2f;
        float elapsedTime = 0f;
        while (elapsedTime < knockbackTime)
        {
            enemy.transform.position += knockbackDirection * Time.deltaTime / knockbackTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        yield return new WaitForSeconds(1.3f); // 1.3f 정지
        Debug.Log("After WaitForSeconds in DamageProcess");

        // Move 상태로 전환
        enemy.SetState(new EnemyMove(enemy));
    }
}