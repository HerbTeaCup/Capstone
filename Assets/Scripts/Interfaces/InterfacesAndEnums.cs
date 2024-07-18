public interface IManager
{
    public void Clear();
}
public interface IUnitDamageable
{
    void TakeDamage(int dmg);
    void Attack();
}

public enum AttackType
{
    straight,
    Radial
}

public interface IWeapon
{
    int AmmoMax { get;}
    int CurrentCapacity { get; }
    int Magazine { get; }
    float ReLoadingTime { get; }
}