using UnityEngine;

public class EnemyIdle : EnemyState
{
    public EnemyIdle(EnemyFSM enemy) : base(enemy) { }

    public override void Enter()
    {
        Debug.Log("Entering Idle State");
        enemy.SetAnimatorParameter("IsIdle", true);
    }

    public override void Execute()
    {
        if (Vector3.Distance(enemy.transform.position, enemy.GetPlayer().position) < enemy.status.FindDistance)
        {
            enemy.TransitionToState(enemy.moveState);
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting Idle State");
        enemy.SetAnimatorParameter("IsIdle", false);
    }
}
