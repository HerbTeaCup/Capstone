
public enum AttackType
{
    straight,
    Radial
}
public enum GameState // 게임 상태
{
    Ready,
    Run,
    Pause,
    GameOver
}
public enum EnemyState
{
    Idle,
    Boundary,
    Capture
}

public interface IinteractableObj
{
    public bool interactable { get; set; }
    public bool calling { get; }

    public void Interaction();
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