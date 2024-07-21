using System.Collections;
using UnityEngine;

public class EnemyDead : EnemyState
{
    public EnemyDead(EnemyFSM enemy) : base(enemy) { }

    public override void Enter()
    {
        Debug.Log("Entering Dead State");
        enemy.status.isDead = true;
        enemy.SetAnimatorParameter("IsDead", true);
        enemy.StopMoving();
        enemy.StartCoroutine(DeadProcess());
    }

    public override void Execute() { }

    public override void Exit() { }

    private IEnumerator DeadProcess()
    {
        enemy.DisableNavMesh();
        yield return new WaitForSeconds(5f);
        GameObject.Destroy(enemy.gameObject);
    }
}
