using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : GenericUnit
{
    [Header("Battle")]
    public bool isReloading = false;
    public int weaponIndex = 0;
    public List<WeaponExtand> weapons = new List<WeaponExtand>();
    public WeaponExtand CurrentWeapon { get { return weapons[weaponIndex]; } }

    [Header("MoveMent")]
    public float speedBlend = 80f;
    public bool isGrounded = false;
    public LayerMask GroundLayer;

    protected override void Start()
    {
        base.Start();
    }
}
