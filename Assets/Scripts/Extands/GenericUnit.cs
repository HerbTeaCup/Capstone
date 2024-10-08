using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenericUnit : MonoBehaviour, IUnitDamageable
{
    [Header("Generic Unit")]
    public int Hp;
    public int MaxHP;
    public Slider hpSlider; // 체력바 UI
    public Text hpText; // 체력값 텍스트로 표시
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
    void StatUIInit()
    {
        hpSlider.minValue = 0; // hp바의 최솟값 설정
        hpSlider.maxValue = MaxHP; // hp바의 최댓값 설정
        hpSlider.value = Hp; // hp바의 현재 값을 체력값으로 설정
    }

    public virtual void TakeDamage(int dmg)
    {
        if (IsAlive == false) return;
            
        Hp -= dmg;

        Debug.Log("Dameged");
    }
}
