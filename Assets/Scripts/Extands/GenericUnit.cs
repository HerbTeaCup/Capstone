using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenericUnit : MonoBehaviour, IUnitDamageable
{
    [Header("Generic Unit")]
    public int Hp;
    public int MaxHP;
    public Slider hpSlider; // ü�¹� UI
    public Text hpText; // ü�°� �ؽ�Ʈ�� ǥ��
    public int weaponIndex = 0;

    public bool IsAlive { get { return Hp > 0; } }

    public float walkSpeed;
    public float runSpeed;
    public float currnetSpeed = 0f; //��Ÿ�ִµ� ��������

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
        hpSlider.minValue = 0; // hp���� �ּڰ� ����
        hpSlider.maxValue = MaxHP; // hp���� �ִ� ����
        hpSlider.value = Hp; // hp���� ���� ���� ü�°����� ����
    }

    public virtual void TakeDamage(int dmg)
    {
        if (IsAlive == false) return;
            
        Hp -= dmg;

        Debug.Log("Dameged");
    }
}
