using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    PlayerStatus _status;

    [Header("Bullet Prefab")]
    //[SerializeField] GameObject Bullet;
    [SerializeField] Transform target; //���� ��� ����
    [SerializeField] Transform firePoint; //���� ���� ����
    [SerializeField] GameObject CloseAttackTrigger; //�������� Collider
    
    bool reloadingTrigger = false;
    bool _closeAttackable = true;

    public Text ammoText; // Bullet Text UI

    void Start()
    {
        _status = GetComponent<PlayerStatus>();

        CloseAttackTrigger.SetActive(false);

        GameManager.Input.InputDelegate += Fire;
        GameManager.Input.InputDelegate += ReLoad;

        UpdateAmmoUI();
    }
    private void Update()
    {
        _status.CurrentWeapon.gameObject.SetActive(GameManager.Input.Aiming);

        CloseAttack();
    }

    void Fire()
    {
        if (_status.IsAlive == false) { return; }
        if (_status.CurrentWeapon.isEmpty)
        {
            reloadingTrigger = true;
            return;
        }
        UpdateAmmoUI(); // �ʱ� bullet ������Ʈ
        if (_status.CurrentWeapon.Magazine < 1) { return; }

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
        UpdateAmmoUI(); // ������Ʈ �ؾ��� �Ѿ��� 0�� ���Ƶ� ���������� ���
    }
    void StraightShoot()
    {
        //�Ѿ� ���� �� ���� ����
        Instantiate(WeaponExtand.Bullet, firePoint.position, firePoint.rotation);
        _status.CurrentWeapon.CurrentCapacity--;

        _status.Sound.WeaponSoundPlay();
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
    }

    void CloseAttack()
    {
        if (GameManager.Input.Aiming == true)
            return;
        if (GameManager.Input.FireTrigger == false)
            return;

        if (_closeAttackable)
        {
            StartCoroutine(HitBoxOnOff());
        }
    }

    void UpdateAmmoUI()
    {
        if (ammoText != null & _status.CurrentWeapon != null)
        {
            ammoText.text = $"{_status.CurrentWeapon.CurrentCapacity} / {_status.CurrentWeapon.Magazine}";
        }
    }

    IEnumerator HitBoxOnOff()
    {
        CloseAttackTrigger.SetActive(true);
        _closeAttackable = false;
        _status.isMoveable = false;

        yield return new WaitForSeconds(0.18f);

        _status.isMoveable = true;
        CloseAttackTrigger.SetActive(false);
        _closeAttackable = true;
    }

    public void Clear()
    {
        GameManager.Input.InputDelegate -= Fire;
    }
}
