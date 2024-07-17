using System.Collections;
using UnityEngine;

public class EnemyIdle : EnemyState
{
    public EnemyIdle(EnemyFSM enemy) : base(enemy) { }

    public override void Enter()
    {
        // Idle ���� ���� �� �ʱ�ȭ �۾�
        Debug.Log("Entering Idle State");
        enemy.SetAnimatorParameter("IsIdle", true);
    }

    public override void Execute()
    {
        // Idle ������ ���� ó��
        if (Vector3.Distance(enemy.transform.position, enemy.GetPlayer().position) < enemy.att.FindDistance)
        {
            enemy.SetState(new EnemyMove(enemy));
        }
    }

    public override void Exit()
    {
        // Idle ���� ���� �� ���� �۾�
        Debug.Log("Exiting Idle State");
        enemy.SetAnimatorParameter("IsIdle", false);
    }

}
