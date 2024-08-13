using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : GenericUnit
{
    [Header("Battle")]
    public bool isReloading = false;

    [Header("MoveMent")]
    public float speedBlend = 10f;
    public float trunSpeedBlend = 20f;
    public float viewSensitivity;
    public bool isGrounded = false;
    public LayerMask GroundLayer;

    protected override void Start()
    {
        base.Start();

        GameManager.Player = this.gameObject;
    }
}
