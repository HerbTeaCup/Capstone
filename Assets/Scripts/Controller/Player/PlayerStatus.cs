using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : GenericUnit
{
    [Header("Battle")]
    public bool isReloading = false;
    [SerializeField] float _invincibleTime = 1.0f;

    float _invincibleDeltaTime = 0f;
    bool _damagedAble { get { return _invincibleDeltaTime > _invincibleTime; } }

    [Header("MoveMent")]
    public float speedBlend = 10f;
    public float trunSpeedBlend = 20f;
    public float viewSensitivity = 1f;
    public bool isGrounded = false;
    public LayerMask GroundLayer;
    [HideInInspector] public Vector3 TargetDir;

    protected override void Start()
    {
        base.Start();

        GameManager.Player = this.gameObject;
    }
    private void Update()
    {
        if (_damagedAble == false)
        {
            _invincibleDeltaTime += Time.deltaTime;
        }
    }

    public override void TakeDamage(int dmg)
    {
        if(_damagedAble == false) { return; }

        _invincibleDeltaTime = 0f;
        base.TakeDamage(dmg);
    }
}
