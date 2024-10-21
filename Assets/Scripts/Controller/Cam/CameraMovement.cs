using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform[] waypoints;  // ī�޶� ���� ��������Ʈ �迭
    public float moveSpeed = 5f;   // ī�޶� �̵� �ӵ�
    public float rotationSpeed = 2f; // ī�޶� ȸ�� �ӵ�

    private int currentWaypointIndex = 0;

    void Update()
    {
        if (waypoints.Length == 0) return;

        // ���� ��ǥ ��������Ʈ�� �̵�
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = targetWaypoint.position - transform.position;
        transform.position += direction.normalized * moveSpeed * Time.deltaTime;

        // ��ǥ ��������Ʈ�� ȸ��
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // ��ǥ ������ �����ϸ� ���� ��������Ʈ�� �̵�
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;  // ������ ��������Ʈ�� �����ϸ� �ٽ� ó������ ����
            }
        }
    }
}
