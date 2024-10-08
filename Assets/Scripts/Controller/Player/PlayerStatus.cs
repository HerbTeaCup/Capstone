using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        // 테스트용도 지울 예정
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10); // 스페이스바를 누르면 10만큼 체력 감소
        }
    }

    public override void TakeDamage(int dmg)
    {
        if (IsAlive == false)
        {
            OptionManager.gameText.text = "Mission Failed!";
            OptionManager.gameText.color = new Color32(0, 0, 0, 255);

            return;
        }

        if (_damagedAble == false) { return; }

        _invincibleDeltaTime = 0f;
        base.TakeDamage(dmg);

        base.hpSlider.value = base.Hp; // HP바의 값을 현재 HP로 설정
        // base.hpText.text = base.Hp + " / " + base.MaxHP;
        base.hpText.text = (float)base.Hp / (float)base.MaxHP * 100.0 + "%";
    }

}
