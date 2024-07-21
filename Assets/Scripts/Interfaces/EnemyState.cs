/*using UnityEngine;

public abstract class EnemyState : IEnemyState
{
    private Animator ani;
    protected readonly EnemyFSM enemy;

    protected EnemyState(EnemyFSM enemy)
    {
        this.enemy = enemy;
    }

    public abstract void Enter();
    public abstract void Execute();
    public abstract void Exit();

    public virtual void TakeDamage(int damage)
    {
        Debug.Log($"ÀûÀÇ HP = {enemy.att.HP}");
        enemy.att.HP -= damage;

        if (enemy.att.HP <= 0)
        {
            enemy.SetState(new EnemyDead(enemy));
        }
        else
        {
            enemy.SetState(new EnemyDamaged(enemy));
        }
    }*/