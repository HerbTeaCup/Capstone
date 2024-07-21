using UnityEngine;

public class EnemyMove : EnemyState
{
    public EnemyMove(EnemyFSM enemy) : base(enemy) { }

    public override void Enter()
    {
        Debug.Log("Entering Move State");
        enemy.SetAnimatorParameter("IsMove", true);
        enemy.MoveTo(enemy.GetPlayer().position);
        enemy.ResumeMoving();
    }

    public override void Execute()
    {
        if (Vector3.Distance(enemy.transform.position, enemy.GetPlayer().position) <= enemy.status.AttackDistance)
        {
            enemy.TransitionToState(enemy.attackState);
        }
        else
        {
            enemy.MoveTo(enemy.GetPlayer().position);
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting Move State");
        enemy.SetAnimatorParameter("IsMove", false);
        enemy.StopMoving();
    }
}
