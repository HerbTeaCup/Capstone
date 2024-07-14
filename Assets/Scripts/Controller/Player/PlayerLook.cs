using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    PlayerStatus _status;

    [SerializeField] Transform TargetPoint;
    [SerializeField] Transform CameraPoint;

    float maxDis = 10f;

    // Start is called before the first frame update
    void Start()
    {
        _status = GetComponent<PlayerStatus>();

        GetPoint();

        GameManager.Input.InputDelegate += PointMove;
        GameManager.Input.InputDelegate += AimCamera;
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

        if (Physics.Raycast(ray, out hit, 50f, _status.GroundLayer))
        {
            float dis = Vector3.Distance(this.transform.position, new Vector3(hit.point.x, this.transform.position.y, hit.point.z));

            if (dis <= maxDis)
            {
                // ������Ʈ�� Ŭ�� �������� �̵�
                TargetPoint.position = hit.point;
            }
            else
            {
                // �Ÿ��� ���� �̻��̸�, ���Ǹ�ŭ ������ ������ ����Ͽ� �̵�
                Vector3 direction = (hit.point - this.transform.position).normalized;
                Vector3 targetPosition = this.transform.position + direction * maxDis;
                TargetPoint.position = targetPosition;
            }

            //0.4f �κ����� �̵��Ͽ� ���� �����̵� ����
            CameraPoint.position = (this.transform.position + TargetPoint.position) * 0.4f;
        }

        Debug.DrawLine(Camera.main.transform.position, hit.point, Color.red);
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
