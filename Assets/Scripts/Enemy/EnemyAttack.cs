using UnityEngine;

public class EnemyAttack : EnemyState
{
    public EnemyAttack(EnemyFSM enemy) : base(enemy) { }

    public override void Enter()
    {
        Debug.Log("Entering Attack State");
        enemy.SetAnimatorParameter("IsAttack", true);
        enemy.StopMoving();
    }

    public override void Execute()
    {
        if (!enemy.status.isDead && Vector3.Distance(enemy.transform.position, enemy.GetPlayer().position) <= enemy.status.AttackDistance)
        {
            if (enemy.CanAttack())
            {
                // enemy.GetPlayer().GetComponent<IUnitDamageable>().TakeDamage(enemy.status.ATK);
                enemy.ResetAttackTime();
            }
            else
            {
                // 공격 지연 시간 감소
                enemy.status.AttackDelay -= Time.deltaTime;
            }
        }
        else
        {
            enemy.TransitionToState(enemy.moveState);
        }
    }

    public override void Exit()
    {
        Debug.Log("Exiting Attack State");
        enemy.SetAnimatorParameter("IsAttack", false);
    }
}
