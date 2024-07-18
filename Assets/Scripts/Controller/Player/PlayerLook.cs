using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    PlayerStatus _status;

    [SerializeField] Transform TargetPoint;
    [SerializeField] Transform CameraPoint;

    float maxDis = 10f;
    float minDis = 3f;

    // Start is called before the first frame update
    void Start()
    {
        _status = GetComponent<PlayerStatus>();

        GetPoint();

        GameManager.Input.InputDelegate += AimCamera;

        GameManager.Input.LateDelegate += PointMove;
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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 50f))
        {
            float dis = Vector3.Distance(this.transform.position, new Vector3(hit.point.x, this.transform.position.y, hit.point.z));
            if (dis <= minDis)
            {
                Vector3 dir = (hit.point - this.transform.position).normalized;
                TargetPoint.position = this.transform.position + dir * minDis;
            }
            else if(dis <= maxDis)
            {
                // ������Ʈ�� Ŭ�� �������� �̵�
                TargetPoint.position = hit.point;
            }
            else
            {
                // �Ÿ��� ���� �̻��̸�, ���Ǹ�ŭ ������ ������ ����Ͽ� �̵�
                Vector3 dir = (hit.point - this.transform.position).normalized;
                TargetPoint.position = this.transform.position + dir * maxDis;
            }

            //0.4f �κ����� �̵��Ͽ� ���� �����̵� ����
            CameraPoint.position = Vector3.Lerp(this.transform.position, TargetPoint.position, 0.4f);

            //���࿡ �����ϸ� ī�޶� ����
            if(_status.CurrentWeapon.fireCurrentRate < _status.CurrentWeapon.FireRate) { return; }
            if (GameManager.Input.FireTrigger && _status.isReloading == false) 
            {
                StopCoroutine(Shake(0.12f, 2f));
                StartCoroutine(Shake(0.12f, 2f));
            }
        }

        Debug.DrawLine(Camera.main.transform.position, hit.point, Color.red);
    }
    IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPosition = CameraPoint.position;//���� ��ġ ����

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
            GameManager.Cam.SetHighestPriority("BasicFollowCam");
        }
    }
}
