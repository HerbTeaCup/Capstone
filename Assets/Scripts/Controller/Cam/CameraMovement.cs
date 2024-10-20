using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform[] waypoints;  // 카메라가 따라갈 웨이포인트 배열
    public float moveSpeed = 5f;   // 카메라 이동 속도
    public float rotationSpeed = 2f; // 카메라 회전 속도

    private int currentWaypointIndex = 0;

    void Update()
    {
        if (waypoints.Length == 0) return;

        // 현재 목표 웨이포인트로 이동
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = targetWaypoint.position - transform.position;
        transform.position += direction.normalized * moveSpeed * Time.deltaTime;

        // 목표 웨이포인트로 회전
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // 목표 지점에 도착하면 다음 웨이포인트로 이동
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;  // 마지막 웨이포인트에 도달하면 다시 처음으로 루프
            }
        }
    }
}
