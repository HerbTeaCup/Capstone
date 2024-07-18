using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    PlayerStatus _status;

    [Header("Bullet Prefab")]
    [SerializeField] GameObject[] Bullet;
    [SerializeField] Transform target; //���� ��� ����
    [SerializeField] Transform firePoint; //���� ���� ����

    bool reloadingTrigger = false;

    // Start is called before the first frame update
    void Start()
    {
        _status = GetComponent<PlayerStatus>();

        GameManager.Input.InputDelegate += Fire;
        GameManager.Input.InputDelegate += ReLoad;
    }
    void Fire()
    {
        if (_status.CurrentWeapon.isEmpty)
        {
            reloadingTrigger = true;
            return;
        }

        if (_status.CurrentWeapon.fireCurrentRate > 0f)
        {
            _status.CurrentWeapon.fireCurrentRate -= Time.deltaTime;
            if (_status.CurrentWeapon.fireCurrentRate < 0.01f) { _status.CurrentWeapon.fireCurrentRate = 0f; }
            return;
        }
        if (GameManager.Input.FireTrigger == false || GameManager.Input.Aiming == false) { return; }

        _status.CurrentWeapon.fireCurrentRate = _status.CurrentWeapon.FireRate;

        // ���콺 ��ġ�� ���� ��ǥ�� ��ȯ
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Mathf.Abs(Camera.main.transform.position.y); // ī�޶��� y ��ġ�� z �࿡ ���� (ī�޶� ���� ������ ���)

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        mouseWorldPosition.y = firePoint.position.y; // ������ ���̷� ����

        // firePoint���� ���콺 ��ġ���� ���� ���� ���
        Vector3 direction = (mouseWorldPosition - firePoint.position).normalized;

        // �Ѿ� ���� �� ���� ����
        Instantiate(Bullet[0], firePoint.position, Quaternion.LookRotation(direction));
        _status.CurrentWeapon.CurrentCapacity--;
        Debug.Log(_status.CurrentWeapon.CurrentCapacity);
    }
    void ReLoad()
    {
        if(reloadingTrigger == false) { return; }

        if (_status.CurrentWeapon.ReLoadingTime > _status.CurrentWeapon.reLoadingDelta) 
        {
            _status.isReloading = true;
            _status.CurrentWeapon.reLoadingDelta += Time.deltaTime;
            return;
        }
        Debug.Log("ReLoading");
        _status.CurrentWeapon.reLoadingDelta = 0f;

        _status.CurrentWeapon.AmmoMax -= _status.CurrentWeapon.Magazine - _status.CurrentWeapon.CurrentCapacity;
        _status.CurrentWeapon.CurrentCapacity = _status.CurrentWeapon.Magazine;

        _status.isReloading = false;
        reloadingTrigger = false;
    }

    public void Clear()
    {
        GameManager.Input.InputDelegate -= Fire;
    }
}
