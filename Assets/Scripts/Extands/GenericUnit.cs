using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenericUnit : MonoBehaviour, IUnitDamageable
{
    [Header("Generic Unit")]
    public int Hp;
    public int MaxHP;
    public Slider hpSlider; // hp slider UI
    public Text hpText; // hp text UI
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
        StatUIInit();
    }

    void StatInit()
    {
        Hp = MaxHP;
    }

    void StatUIInit() // Status UI 초기화
    {
        // hpSlider가 할당되지 않았을 경우
        if (hpSlider == null)
        {
            Debug.LogWarning("The hpSlider is not assigned in the Inspector!");
            return;
        }
        hpSlider.minValue = 0; // hp 최솟값
        hpSlider.maxValue = MaxHP; // hp 최댓값
        hpSlider.value = Hp; // hp 현재 값
    }

    public virtual void TakeDamage(int dmg)
    {
        if (IsAlive == false)
            return;

        Hp -= dmg;

        Debug.Log("Damaged");
    }
}
