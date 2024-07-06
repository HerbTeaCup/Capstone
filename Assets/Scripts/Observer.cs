using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    public Transform player; //플레이어의 위치에 접근. 플레이어에게 시선이 닿는지 감지 가능
    public GameEnding gameEnding;

    bool m_IsPlayerInRange;

    void OnTriggerEnter(Collider other) // 플레이어 캐릭터 감지
    {
        // OnTriggerEnter가 호출될 때마다 플레이어가 실제로 공격 범위 내에 있는지 확인
        if (other.transform == player)
        {
            m_IsPlayerInRange = true;
        }
    }
    void OnTriggerExit(Collider other) // 플레이어 캐릭터 감지 X
    {
        if (other.transform == player)
        {
            m_IsPlayerInRange = false;
        }
    }

    void Update()
    {
        if (m_IsPlayerInRange) // 실제로 플레이어가 범위 안에 있는지
        {
            Vector3 direction = player.position - transform.position + Vector3.up; // 방향 생성
            Ray ray = new Ray(transform.position, direction); // ray 생성
            RaycastHit raycastHit;

            if (Physics.Raycast(ray, out raycastHit))
            {
                if (raycastHit.collider.transform == player)
                {
                    gameEnding.CaughtPlayer();
                }
            }
        }
    }
}
