using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : WeaponExtand
{
    protected override void Start()
    {
        base.Start();
        Init(AttackType.straight, 14, 7, 1.3f, 0.9f);
    }
    private void Update()
    {
        Debug.Log(this.CurrentCapacity);
    }
}
