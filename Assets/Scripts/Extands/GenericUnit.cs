using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericUnit : MonoBehaviour, IUnitDamageable
{
    [Header("Generic Unit")]
    public int Hp;
    public int MaxHP;

    public bool IsAlive { get { return Hp > 0; } }

    public float walkSpeed;
    public float runSpeed;
    public float currnetSpeed = 0f; //오타있는데 수정안함

    // Start is called before the first frame update
    protected virtual void Start()
    {
        StatInit();
    }

    void StatInit()
    {
        Hp = MaxHP;
    }
    public virtual void TakeDamage(int dmg)
    {
        if (IsAlive == false)
            return;

        Hp -= dmg;

        Debug.Log("Dameged");
    }
}
