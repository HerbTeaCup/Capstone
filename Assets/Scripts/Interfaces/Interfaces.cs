public interface IManager
{
    public void Clear();
}
public interface IUnitDamageable
{
    void TakeDamage(int dmg);
    void Attack();
}