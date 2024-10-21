using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraWaypointMove : MonoBehaviour
{
    public string sceneToLoad = "OtherScene"; // �ٸ� �� �̸�
    public float moveSpeed = 5f;   // ī�޶� �̵� �ӵ�
    public float rotationSpeed = 2f; // ī�޶� ȸ�� �ӵ�

    private Transform[] waypoints;  // �ε�� ���� ��������Ʈ �迭
    private int currentWaypointIndex = 0;  // ���� �̵� ���� ��������Ʈ �ε���
    private Transform targetWaypoint;      // ��ǥ ��������Ʈ

    void Start()
    {
        // �ٸ� ���� Additive�� �ε�
        SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive).completed += OnSceneLoaded;
    }

    // �� �ε� �Ϸ� �� ��������Ʈ ����
    void OnSceneLoaded(AsyncOperation op)
    {
        // �ε�� ������ "Waypoint" �±׷� ��������Ʈ ������Ʈ ã��
        GameObject[] waypointObjects = GameObject.FindGameObjectsWithTag("Waypoint");
        waypoints = new Transform[waypointObjects.Length];
        for (int i = 0; i < waypointObjects.Length; i++)
        {
            waypoints[i] = waypointObjects[i].transform;
        }

        // ù ��° ��������Ʈ�� ��ǥ�� ����
        if (waypoints.Length > 0)
        {
            targetWaypoint = waypoints[currentWaypointIndex];
        }
    }

    void Update()
    {
        if (targetWaypoint == null) return;

        // ��������Ʈ�� �̵�
        MoveTowardsWaypoint();

        // ��������Ʈ�� �����ߴ��� üũ
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            // ���� ��������Ʈ�� �̵�
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;  // ������ ��������Ʈ�� �����ϸ� ó������ ���ư��� (����)
            }
            targetWaypoint = waypoints[currentWaypointIndex];
        }
    }

    void MoveTowardsWaypoint()
    {
        // ��ǥ ��������Ʈ�� �̵�
        Vector3 direction = targetWaypoint.position - transform.position;
        transform.position += direction.normalized * moveSpeed * Time.deltaTime;

        // ��ǥ ��������Ʈ�� ���� �ε巴�� ȸ��
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
