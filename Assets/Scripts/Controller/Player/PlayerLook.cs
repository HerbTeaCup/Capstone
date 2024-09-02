using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    PlayerStatus _status;

    [SerializeField] Transform CameraArm;
    [SerializeField] Transform TargetPoint;
    [SerializeField] Transform CameraPoint;

    float maxDis = 10f;
    float minDis = 3f;

    //camera
    float _targetPitch = 0f;

    // Start is called before the first frame update
    void Start()
    {
        _status = GetComponent<PlayerStatus>();

        GetPoint();

        GameManager.Input.InputDelegate += AimCamera;
        GameManager.Input.LateDelegate += PointMove;
        GameManager.Input.LateDelegate += CameraRotate;
    }
    void GetPoint()
    {
        if (CameraPoint != null) { return; }

        foreach (Transform child in this.transform)
        {
            if (child.name == "CameraPos")
            {
                CameraPoint = child;
            }
            if (child.name == "TargetPos")
            {
                TargetPoint = child;
                return;
            }
        }
    }
    void PointMove()
    {
        if(GameManager.Input.Aiming == false) { return; }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 50f, 1<<31))
        {
            float dis = Vector3.Distance(this.transform.position, new Vector3(hit.point.x, this.transform.position.y, hit.point.z));
            if (dis <= minDis)
            {
                Vector3 dir = (hit.point - this.transform.position).normalized;
                TargetPoint.position = this.transform.position + dir * minDis;
            }
            else if(dis <= maxDis)
            {
                // 오브젝트를 마우스 지점으로 이동
                TargetPoint.position = hit.point;
            }
            else
            {
                // 거리가 조건 이상이면, 조건만큼 떨어진 지점을 계산하여 이동
                Vector3 dir = (hit.point - this.transform.position).normalized;
                TargetPoint.position = this.transform.position + dir * maxDis;
            }

            //0.4f 부분으로 이동하여 과한 시점이동 방지
            CameraPoint.position = Vector3.Lerp(this.transform.position, TargetPoint.position, 0.4f);
            CameraPoint.position = new Vector3(CameraPoint.position.x, this.transform.position.y, CameraPoint.position.z);

            //만약에 공격하면 카메라 흔들기
            if(_status.CurrentWeapon.fireCurrentRate < _status.CurrentWeapon.FireRate) { return; }
            if (GameManager.Input.FireTrigger && _status.isReloading == false) 
            {
                StopCoroutine(Shake(0.12f, 0.5f));
                StartCoroutine(Shake(0.12f, 0.5f));
            }
        }

        Debug.DrawLine(Camera.main.transform.position, hit.point, Color.red);
    }
    void CameraRotate()
    {
        if (GameManager.Input.ViewMove != 0f)
        {
            _targetPitch += GameManager.Input.ViewMove * _status.viewSensitivity;
        }

        _targetPitch = AngleClamp(float.MinValue, float.MaxValue, _targetPitch);

        CameraArm.rotation = Quaternion.Euler(0, _targetPitch, 0);
        CameraPoint.rotation = CameraArm.rotation;
    }
    float AngleClamp(float min, float max, float value)
    {
        if (value < -360) value += 360;
        if (value > 360) value -= 360;

        return Mathf.Clamp(value, min, max);
    }
    IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPosition = CameraPoint.position;//원래 위치 저장

        float deltaTime = 0.0f;

        while (deltaTime < duration)
        {
            CameraPoint.position = originalPosition + Random.insideUnitSphere * magnitude;

            deltaTime += Time.deltaTime;

            yield return null;
        }

        CameraPoint.position = originalPosition;
    }
    void AimCamera()
    {
        if (GameManager.Input.Aiming)
        {
            GameManager.Cam.SetHighestPriority("AimingCam");
        }
        else
        {
            GameManager.Cam.SetHighestPriority("3rd Person Cam");
        }
    }
}
