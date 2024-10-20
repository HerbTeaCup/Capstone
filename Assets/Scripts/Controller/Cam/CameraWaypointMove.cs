using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraWaypointMove : MonoBehaviour
{
    public string sceneToLoad = "OtherScene"; // 다른 씬 이름
    public float moveSpeed = 5f;   // 카메라 이동 속도
    public float rotationSpeed = 2f; // 카메라 회전 속도

    private Transform[] waypoints;  // 로드된 씬의 웨이포인트 배열
    private int currentWaypointIndex = 0;  // 현재 이동 중인 웨이포인트 인덱스
    private Transform targetWaypoint;      // 목표 웨이포인트

    void Start()
    {
        // 다른 씬을 Additive로 로드
        SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive).completed += OnSceneLoaded;
    }

    // 씬 로드 완료 후 웨이포인트 참조
    void OnSceneLoaded(AsyncOperation op)
    {
        // 로드된 씬에서 "Waypoint" 태그로 웨이포인트 오브젝트 찾기
        GameObject[] waypointObjects = GameObject.FindGameObjectsWithTag("Waypoint");
        waypoints = new Transform[waypointObjects.Length];
        for (int i = 0; i < waypointObjects.Length; i++)
        {
            waypoints[i] = waypointObjects[i].transform;
        }

        // 첫 번째 웨이포인트를 목표로 설정
        if (waypoints.Length > 0)
        {
            targetWaypoint = waypoints[currentWaypointIndex];
        }
    }

    void Update()
    {
        if (targetWaypoint == null) return;

        // 웨이포인트로 이동
        MoveTowardsWaypoint();

        // 웨이포인트에 도착했는지 체크
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            // 다음 웨이포인트로 이동
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;  // 마지막 웨이포인트에 도착하면 처음으로 돌아가기 (루프)
            }
            targetWaypoint = waypoints[currentWaypointIndex];
        }
    }

    void MoveTowardsWaypoint()
    {
        // 목표 웨이포인트로 이동
        Vector3 direction = targetWaypoint.position - transform.position;
        transform.position += direction.normalized * moveSpeed * Time.deltaTime;

        // 목표 웨이포인트를 향해 부드럽게 회전
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
