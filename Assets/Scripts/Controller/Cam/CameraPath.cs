using UnityEngine;

public class CameraPath : MonoBehaviour
{
    public Transform pointA;  // 시작점
    public Transform pointB;  // 경로의 중간 지점
    public Transform pointC;  // 끝점

    public float speed = 0.5f;     // 카메라 이동 속도
    public float rotationSpeed = 2f; // 회전 속도 조정
    private float t = 0f;          // 경로 상의 진행 상태 (0 ~ 1)

    void Update()
    {
        // 카메라가 베지어 곡선을 따라 이동하도록 설정
        t += Time.deltaTime * speed;
        if (t > 1f) t = 1f;  // t 값이 1을 넘지 않도록 제한

        // Bezier 곡선 수식: (1 - t)^2 * A + 2(1 - t)t * B + t^2 * C
        Vector3 position = Mathf.Pow(1 - t, 2) * pointA.position +
                           2 * (1 - t) * t * pointB.position +
                           Mathf.Pow(t, 2) * pointC.position;

        transform.position = position;

        // 목표 방향으로 부드럽게 회전
        Quaternion targetRotation = Quaternion.LookRotation(pointC.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
