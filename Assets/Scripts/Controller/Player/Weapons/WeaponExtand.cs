using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponExtand : MonoBehaviour, IWeapon
{
    public AttackType type;
    public int AmmoMax { get; set; }//������ �ִ� �Ѿ� ��
    public int CurrentCapacity { get; set; }//���� źâ�� Magazine�� ���� �� ����
    public int Magazine { get; private set; }//��ü źâ�� �Ϲ������� 30
    public float ReLoadingTime { get; private set; }
    public float FireRate { get; private set; }

    [HideInInspector] public float reLoadingDelta = 0f;
    [HideInInspector] public float fireCurrentRate = 0f;
    public bool isEmpty { get { return CurrentCapacity < 1; } }


    protected void Init(AttackType type, int AmmoMax, int Magazine, float ReLoadingTime, float fireRate)
    {
        this.type = type;
        this.AmmoMax = AmmoMax;
        this.Magazine = Magazine;
        this.ReLoadingTime = ReLoadingTime;
        this.FireRate = fireRate;
    }
}
