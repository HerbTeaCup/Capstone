using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    PlayerStatus _status;

    [Header("Bullet Prefab")]
    //[SerializeField] GameObject Bullet;
    [SerializeField] Transform target; //���� ��� ����
    [SerializeField] Transform firePoint; //���� ���� ����

    bool reloadingTrigger = false;

    public Text ammoText; // �Ѿ� ���� UI

    // Start is called before the first frame update
    void Start()
    {
        _status = GetComponent<PlayerStatus>();

        GameManager.Input.InputDelegate += Fire;
        GameManager.Input.InputDelegate += ReLoad;
   
        UpdateAmmoUI(); // �Ѿ� ���� �ʱ�ȭ
    }
    void Fire()
    {
        if (_status.IsAlive == false) { return; }
        if (_status.CurrentWeapon.isEmpty)
        {
            reloadingTrigger = true;
            return;
        }
        if (_status.CurrentWeapon.AmmoMax < 1) { return; }

        if (_status.CurrentWeapon.fireCurrentRate > 0f)
        {
            _status.CurrentWeapon.fireCurrentRate -= Time.deltaTime;
            if (_status.CurrentWeapon.fireCurrentRate < 0.01f) { _status.CurrentWeapon.fireCurrentRate = 0f; }
            return;
        }
        if (GameManager.Input.FireTrigger == false || GameManager.Input.Aiming == false) { return; }

        _status.CurrentWeapon.fireCurrentRate = _status.CurrentWeapon.FireRate;

        switch (_status.CurrentWeapon.type)
        {
            case AttackType.straight:
                StraightShoot();
                break;
            case AttackType.Radial:
                RadialShoot();
                break;
        }
        UpdateAmmoUI(); // �Ѿ� ���� �ʱ�ȭ
    }
    void StraightShoot()
    {
        //�Ѿ� ���� �� ���� ����
        Instantiate(WeaponExtand.Bullet, firePoint.position, firePoint.rotation);
        _status.CurrentWeapon.CurrentCapacity--;
    }
    void RadialShoot()
    {
        ShotGun temp = (ShotGun) _status.CurrentWeapon;
        for (int i = 0; i < temp.bulletCount; i++)
        {
            // źȯ�� ���� ������ �����ϰ� ����
            float angle = Random.Range(-temp.spreadAngle / 2, temp.spreadAngle / 2);

            // firePoint�� ȸ������ �������� ������ ����
            Quaternion rotation = firePoint.rotation * Quaternion.Euler(0, angle, 0);

            // źȯ ����
            Instantiate(WeaponExtand.Bullet, firePoint.position, rotation);
        }
        temp.CurrentCapacity--;
    }
    void ReLoad()
    {
        if(reloadingTrigger == false) { return; }
        if (_status.CurrentWeapon.AmmoMax < 1) { return; }

        if (_status.CurrentWeapon.ReLoadingTime > _status.CurrentWeapon.reLoadingDelta) 
        {
            _status.isReloading = true;
            _status.CurrentWeapon.reLoadingDelta += Time.deltaTime;
            return;
        }
        Debug.Log("Player ReLoading");
        _status.CurrentWeapon.reLoadingDelta = 0f;

        _status.CurrentWeapon.AmmoMax -= _status.CurrentWeapon.Magazine - _status.CurrentWeapon.CurrentCapacity;
        _status.CurrentWeapon.CurrentCapacity = _status.CurrentWeapon.Magazine;

        _status.isReloading = false;
        reloadingTrigger = false;

        UpdateAmmoUI(); // �Ѿ� ���� �ʱ�ȭ
    }

    // �Ѿ� ���� UI ������Ʈ �Լ�
    void UpdateAmmoUI()
    {
        if (ammoText != null && _status.CurrentWeapon != null)
        {
            ammoText.text = $"{_status.CurrentWeapon.CurrentCapacity} / {_status.CurrentWeapon.Magazine} (Max: {_status.CurrentWeapon.AmmoMax})";
        }
    }

    public void Clear()
    {
        GameManager.Input.InputDelegate -= Fire;
    }
}
