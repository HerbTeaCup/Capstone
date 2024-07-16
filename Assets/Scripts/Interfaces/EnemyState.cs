using UnityEngine;
public abstract class EnemyState : IEnemyState
{
    public float FindDistance { get; set; } = 8f;
    public float AttackDistance { get; set; } = 2f;
    public float MoveSpeed { get; set; } = 5f;
    public int ATK { get; set; } = 5;
    public int HP { get; set; } = 100;
    public float AttackDelay { get; set; } = 2f;
    public float CurrentTime { get; set; } = 0f;

    protected readonly EnemyFSM enemy;

    protected EnemyState(EnemyFSM enemy)
    {
        this.enemy = enemy;
        this.HP = enemy.HP; // Intialize state HP from enemy HP
    }

    public abstract void Enter();
    public abstract void Execute();
    public abstract void Exit();

    public virtual void TakeDamage(int damage)
    {
        Debug.Log($"ÀûÀÇ HP = {HP}");
        HP -= damage;
        enemy.HP = HP;  // Update enemy HP
        if (HP <= 0)
        {
            enemy.SetState(new EnemyDead(enemy));
        }
        else
        {
            enemy.SetState(new EnemyDamaged(enemy));
        }
    }
}