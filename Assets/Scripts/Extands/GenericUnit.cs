using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenericUnit : MonoBehaviour, IUnitDamageable
{
    [Header("Generic Unit")]
    public float Hp;
    public float MaxHP;
    /*public Slider hpSlider; // hp slider UI
    public Text hpText; // hp text UI*/
    public ImgsFillDynamic hpGauge;
    public int weaponIndex = 0;

    public bool IsAlive { get { return Hp > 0; } }

    public float walkSpeed;
    public float runSpeed;
    public float currnetSpeed = 0f; //오타있는데 수정안함


    public List<WeaponExtand> weapons = new List<WeaponExtand>();
    public WeaponExtand CurrentWeapon { get { return weapons[weaponIndex]; } }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        StatInit();
        UpdateHPGauge();
    }

    void StatInit()
    {
        Hp = MaxHP;
    }

    public virtual void TakeDamage(int dmg)
    {
        if (IsAlive == false)
            return;

        Hp -= dmg;
        Hp = Mathf.Clamp(Hp, 0, MaxHP); // HP가 0 미만으로 내려가지 않도록 제한
        UpdateHPGauge(); // HP 변경 시 게이지 업데이트

        Debug.Log("Damaged");
    }

    void UpdateHPGauge()
    {
        if (hpGauge != null)
        {
            float hpRatio = Hp / MaxHP; // 현재 HP 비율 계산
            hpGauge.SetValue(hpRatio); // Simple Round Gauge에 비율 적용
        }
    }
}
