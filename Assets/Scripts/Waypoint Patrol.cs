using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // Nav Mesh�� ���õ� ��Ʈ���� ó��

public class WaypointPatrol : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform[] waypoints; // ���� ��ȸ�ϴ� ��������Ʈ

    int currentWaypoints; // ���� ��������Ʈ
    void Start()
    {
        agent.SetDestination(waypoints[0].position); // Nav Mesh Agent�� ���� ������ ����
    }

    void Update()
    {
        // Nav Mesh Agent�� �������� �����ߴ��� Ȯ��
        if (agent.remainingDistance < agent.stoppingDistance) // ���������� ���� �Ÿ��� �����Ÿ����� ª���� Ȯ��
        {
            currentWaypoints = (currentWaypoints + 1) % waypoints.Length; // ���� �ε��� ������Ʈ�� ���� Agent�� ������ ����
            agent.SetDestination(waypoints[currentWaypoints].position); // 0�� �ε����� ����ϴ� ��� ������ ���� ������ ��������Ʈ�� �ε����� ���
        }
    }
}
