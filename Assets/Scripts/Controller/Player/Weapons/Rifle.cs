using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : WeaponExtand
{

    private void Start()
    {
        Init(AttackType.straight, 240, 30, 1.3f, 0.1f);
    }
}
