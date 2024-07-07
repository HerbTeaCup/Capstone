using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [Header("Battle")]
    public int Hp;
    public int MaxHP;
    [Header("MoveMent")]
    public float walkSpeed;
    public float runSpeed;
    public float currnetSpeed = 0f;
    public float speedBlend = 80f;
    public bool isGrounded = false;
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
