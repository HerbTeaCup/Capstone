using UnityEngine;

public class CameraPath : MonoBehaviour
{
    public Transform pointA;  // ������
    public Transform pointB;  // ����� �߰� ����
    public Transform pointC;  // ����

    public float speed = 0.5f;     // ī�޶� �̵� �ӵ�
    public float rotationSpeed = 2f; // ȸ�� �ӵ� ����
    private float t = 0f;          // ��� ���� ���� ���� (0 ~ 1)

    void Update()
    {
        // ī�޶� ������ ��� ���� �̵��ϵ��� ����
        t += Time.deltaTime * speed;
        if (t > 1f) t = 1f;  // t ���� 1�� ���� �ʵ��� ����

        // Bezier � ����: (1 - t)^2 * A + 2(1 - t)t * B + t^2 * C
        Vector3 position = Mathf.Pow(1 - t, 2) * pointA.position +
                           2 * (1 - t) * t * pointB.position +
                           Mathf.Pow(t, 2) * pointC.position;

        transform.position = position;

        // ��ǥ �������� �ε巴�� ȸ��
        Quaternion targetRotation = Quaternion.LookRotation(pointC.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
