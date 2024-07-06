using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // Nav Mesh와 관련된 스트립팅 처리

public class WaypointPatrol : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform[] waypoints; // 적이 순회하는 웨이포인트

    int currentWaypoints; // 현재 웨이포인트
    void Start()
    {
        agent.SetDestination(waypoints[0].position); // Nav Mesh Agent의 최초 목적지 설정
    }

    void Update()
    {
        // Nav Mesh Agent가 목적지에 도착했는지 확인
        if (agent.remainingDistance < agent.stoppingDistance) // 목적지까지 남은 거리가 정지거리보다 짧은지 확인
        {
            currentWaypoints = (currentWaypoints + 1) % waypoints.Length; // 현재 인덱스 업데이트한 다음 Agent의 목적지 설정
            agent.SetDestination(waypoints[currentWaypoints].position); // 0을 인덱스로 사용하는 대신 유령이 현재 도달한 웨이포인트를 인덱스로 사용
        }
    }
}
