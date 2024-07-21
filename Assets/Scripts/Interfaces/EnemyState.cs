using UnityEngine;

public abstract class EnemyState
{
    protected EnemyFSM enemy;

    protected EnemyState(EnemyFSM enemy)
    {
        this.enemy = enemy;
    }

    public abstract void Enter();
    public abstract void Execute();
    public abstract void Exit();

    public virtual void TakeDamage(int damage)
    {
        enemy.status.Hp -= damage;

        if (enemy.status.Hp <= 0)
        {
            enemy.TransitionToState(enemy.deadState);
        }
        else
        {
            enemy.TransitionToState(enemy.damagedState);
        }
    }
}
