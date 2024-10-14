using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStatus : GenericUnit
{
    [Header("Battle")]
    public bool isReloading = false;
    [SerializeField] bool _powerOverwheming;
    [SerializeField] float _invincibleTime = 1.0f;

    float _invincibleDeltaTime = 0f;
    bool _damagedAble { get { return _invincibleDeltaTime > _invincibleTime; } }

    [Header("MoveMent")]
    public float speedBlend = 10f;
    public float trunSpeedBlend = 20f;
    public float viewSensitivity = 1f;
    public bool isGrounded = false;
    public bool excuting = false;
    public LayerMask GroundLayer;
    [HideInInspector] public Transform ExcuteTransform;
    [HideInInspector] public Vector3 TargetDir;


    protected override void Start()
    {
        base.Start();
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
        if (_powerOverwheming) { return; }
        if(_damagedAble == false) { return; }

        _invincibleDeltaTime = 0f;
        base.TakeDamage(dmg);

        // hpSlider와 hpText가 null이 아닐 경우에 UI 넣기
        if (base.hpSlider != null) base.hpSlider.value = base.Hp;
        if (base.hpText != null) base.hpText.text = (float)base.Hp / (float)base.MaxHP * 100.0 + "%";
        // base.hpText.text = base.Hp + " / " + base.MaxHp;
    }
}
