using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    PlayerStatus _status;
    CameraShake _shake;

    [Header("Bullet Prefab")]
    [SerializeField] GameObject[] Bullet;
    [SerializeField] Transform target; //공격 대상 지점
    [SerializeField] Transform firePoint; //공격 시작 지점

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

        // 마우스 위치를 월드 좌표로 변환
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Mathf.Abs(Camera.main.transform.position.y); // 카메라의 y 위치를 z 축에 설정 (카메라가 고정 높이일 경우)

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        mouseWorldPosition.y = firePoint.position.y; // 동일한 높이로 설정

        // firePoint에서 마우스 위치로의 방향 벡터 계산
        Vector3 direction = (mouseWorldPosition - firePoint.position).normalized;

        // 총알 생성 및 방향 설정
        Instantiate(Bullet[0], firePoint.position, Quaternion.LookRotation(direction));

        // 카메라 흔들림 효과
        _shake.OnCameraShake(10f, 10f);
    }

    public void Clear()
    {
        GameManager.Input.InputDelegate -= Fire;
    }
}
