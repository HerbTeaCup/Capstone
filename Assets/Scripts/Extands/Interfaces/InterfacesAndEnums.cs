
using UnityEngine;

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

public interface IinteractableObj
{
    public bool interactable { get; set; }
    public bool calling { get; }
    public bool ui_Show { get; set; }

    public void Interaction();
    public void UpdateUIPosition(Camera camera);
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