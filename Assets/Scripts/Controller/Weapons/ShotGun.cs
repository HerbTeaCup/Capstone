using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : WeaponExtand
{
    public int bulletCount { get; private set; } = 6;
    public float spreadAngle { get; private set; } = 15f;
    // Start is called before the first frame update
    void Start()
    {
        Init(AttackType.Radial, 40, 5, 3f, 1.5f);
    }
}
