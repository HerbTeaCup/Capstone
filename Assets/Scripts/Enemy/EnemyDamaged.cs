using System.Collections;
using UnityEngine;

public class EnemyDamaged : EnemyState
{
    public EnemyDamaged(EnemyFSM enemy) : base(enemy) { }

    private float knockbackDistance = 2f;
    private Vector3 knockbackDirection;

    public override void Enter()
    {
        Debug.Log("Entering Damaged State");
        enemy.SetAnimatorParameter("IsDamaged", true);
        enemy.StopMoving();

        knockbackDirection = (enemy.transform.position - enemy.GetPlayer().position).normalized * knockbackDistance;

        enemy.StartCoroutine(DamagedProcess());
    }

    public override void Execute() { }

    public override void Exit()
    {
        Debug.Log("Exiting Damaged State");
        enemy.SetAnimatorParameter("IsDamaged", false);
        enemy.ResumeMoving();
    }

    private IEnumerator DamagedProcess()
    {
        float knockbackTime = 0.2f;
        float elapsedTime = 0f;
        while (elapsedTime < knockbackTime)
        {
            enemy.transform.position += knockbackDirection * Time.deltaTime / knockbackTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1.3f);

        enemy.TransitionToState(enemy.moveState);
    }
}
