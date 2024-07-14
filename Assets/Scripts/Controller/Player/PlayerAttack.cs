using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    PlayerStatus _status;
    CameraShake _shake;

    [Header("Bullet Prefab")]
    [SerializeField] GameObject[] Bullet;
    [SerializeField] Transform target; //���� ��� ����
    [SerializeField] Transform firePoint; //���� ���� ����

    // Start is called before the first frame update
    void Start()
    {
        _status = GetComponent<PlayerStatus>();
        _shake = Camera.main.GetComponent<CameraShake>();

        GameManager.Input.InputDelegate += Fire;
    }

    //void Fire()
    //{
    //    if (_status.fireCurrentRate > 0f)
    //    {
    //        _status.fireCurrentRate -= Time.deltaTime;
    //        if (_status.fireCurrentRate < 0.01f) { _status.fireCurrentRate = 0f; }
    //        return;
    //    }
    //    if (GameManager.Input.FireTrigger == false || GameManager.Input.Aiming == false) { return; }

    //    _status.fireCurrentRate = _status.fireRate;

    //    HS_ProjectileMover temp = Instantiate(Bullet[0], firePoint.position, this.transform.rotation).GetComponent<HS_ProjectileMover>();
    //    _shake.OnCameraShake(10f, 10f);
    //    temp = null;
    //}
    void Fire()
    {
        if (_status.fireCurrentRate > 0f)
        {
            _status.fireCurrentRate -= Time.deltaTime;
            if (_status.fireCurrentRate < 0.01f) { _status.fireCurrentRate = 0f; }
            return;
        }
        if (GameManager.Input.FireTrigger == false || GameManager.Input.Aiming == false) { return; }

        _status.fireCurrentRate = _status.fireRate;

        // ���콺 ��ġ�� ���� ��ǥ�� ��ȯ
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Mathf.Abs(Camera.main.transform.position.y); // ī�޶��� y ��ġ�� z �࿡ ���� (ī�޶� ���� ������ ���)

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        mouseWorldPosition.y = firePoint.position.y; // ������ ���̷� ����

        // firePoint���� ���콺 ��ġ���� ���� ���� ���
        Vector3 direction = (mouseWorldPosition - firePoint.position).normalized;

        // �Ѿ� ���� �� ���� ����
        Instantiate(Bullet[0], firePoint.position, Quaternion.LookRotation(direction));

        // ī�޶� ��鸲 ȿ��
        _shake.OnCameraShake(10f, 10f);
    }

    public void Clear()
    {
        GameManager.Input.InputDelegate -= Fire;
    }
}
