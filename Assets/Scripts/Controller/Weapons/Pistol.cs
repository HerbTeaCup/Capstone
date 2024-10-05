using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : WeaponExtand
{
    protected override void Start()
    {
        base.Start();
        Init(AttackType.straight, 12, 6, 1.3f, 1.3f);
    }
}
