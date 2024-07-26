
public enum AttackType
{
    straight,
    Radial
}
public enum EnemyState
{
    Idle,
    Boundary,
    Capture
}

public interface IManager
{
    public void Clear();
}
public interface IUnitDamageable
{
    void TakeDamage(int dmg);
}
public interface IWeaponGun
{
    int AmmoMax { get;}
    int CurrentCapacity { get; }
    int Magazine { get; }
    float ReLoadingTime { get; }
}