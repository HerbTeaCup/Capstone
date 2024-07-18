using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [Header("Battle")]
    public bool isReloading = false;
    public int Hp;
    public int MaxHP;
    public int weaponIndex = 0;
    public List<WeaponExtand> weapons = new List<WeaponExtand>();
    public WeaponExtand CurrentWeapon { get { return weapons[weaponIndex]; } }

    [Header("MoveMent")]
    public float walkSpeed;
    public float runSpeed;
    public float currnetSpeed = 0f; //오타있는데 수정안함
    public float speedBlend = 80f;
    public bool isGrounded = false;
    public LayerMask GroundLayer;

    [Header("View")]
    public float viewMaxSpeed = 10f;

    private void Start()
    {
        StatInit();
    }

    void StatInit()
    {
        Hp = MaxHP;
    }
}
