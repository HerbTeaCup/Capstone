using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    PlayerStatus _status;

    [Header("Bullet Prefab")]
    //[SerializeField] GameObject Bullet;
    [SerializeField] Transform target; //공격 대상 지점
    [SerializeField] Transform firePoint; //공격 시작 지점

    // Stealth UI
    [SerializeField] float stealthRange =  0.7f;
    [SerializeField] KeyCode stealthInteractionKey = KeyCode.F;
    [SerializeField] GameObject stealthUI;

    bool reloadingTrigger = false;

    // Start is called before the first frame update
    void Start()
    {
        _status = GetComponent<PlayerStatus>();

        GameManager.Input.InputDelegate += Fire;
        GameManager.Input.InputDelegate += ReLoad;
        GameManager.Input.InputDelegate += Stealth;
    }

    public void Stealth()
    {
        // Enemy tag가 되어 있는 오브젝트를 찾음
        Collider[] enemies = Physics.OverlapSphere(transform.position, stealthRange, LayerMask.GetMask("Enemy"));

        foreach (var enemy in enemies)
        {
            var enemyStatus = enemy.GetComponent<EnemyStatus>();
            if (enemyStatus.Hp == 0)
            {
                Debug.Log("EnemyStatus is null!");
                continue; // 다음 적으로 넘어가기
            }

            Vector3 directionToEnemy = (enemy.transform.position - transform.position).normalized; 
            float dotProduct = Vector3.Dot(transform.forward, directionToEnemy); // 플레이어가 적을 바라보고 있는지 확인

            if (dotProduct > 0) // 플레이어가 적의 등을 보는 경우
            {
                // 스텔스 킬 UI 표시 및 상호작용 처리
                ShowStealthUI(true);

                if (Input.GetKeyDown(stealthInteractionKey))
                {
                    enemy.GetComponent<EnemyStatus>().Hp = 0; // 적의 체력 0으로 만듦
                    ShowStealthUI(false); // UI 숨김
                    Debug.Log("Enemy Hp = 0");
                }
                return;
            }
        }

        ShowStealthUI(false); // 적이 없으면 UI 숨김
    }

    void ShowStealthUI(bool show)
    {
        stealthUI.SetActive(show); // 스텔스 텍스트 UI 표시/숨기기
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

    }
    void StraightShoot()
    {
        //총알 생성 및 방향 설정
        Instantiate(WeaponExtand.Bullet, firePoint.position, firePoint.rotation);
        _status.CurrentWeapon.CurrentCapacity--;
    }
    void RadialShoot()
    {
        ShotGun temp = (ShotGun) _status.CurrentWeapon;
        for (int i = 0; i < temp.bulletCount; i++)
        {
            // 탄환의 퍼짐 각도를 랜덤하게 설정
            float angle = Random.Range(-temp.spreadAngle / 2, temp.spreadAngle / 2);

            // firePoint의 회전값을 기준으로 각도를 적용
            Quaternion rotation = firePoint.rotation * Quaternion.Euler(0, angle, 0);

            // 탄환 생성
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

    public void Clear()
    {
        GameManager.Input.InputDelegate -= Fire;
        GameManager.Input.InputDelegate -= Stealth;
    }
}
