using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    public Transform player; //�÷��̾��� ��ġ�� ����. �÷��̾�� �ü��� ����� ���� ����
    public GameEnding gameEnding;

    bool m_IsPlayerInRange;

    void OnTriggerEnter(Collider other) // �÷��̾� ĳ���� ����
    {
        // OnTriggerEnter�� ȣ��� ������ �÷��̾ ������ ���� ���� ���� �ִ��� Ȯ��
        if (other.transform == player)
        {
            m_IsPlayerInRange = true;
        }
    }
    void OnTriggerExit(Collider other) // �÷��̾� ĳ���� ���� X
    {
        if (other.transform == player)
        {
            m_IsPlayerInRange = false;
        }
    }

    void Update()
    {
        if (m_IsPlayerInRange) // ������ �÷��̾ ���� �ȿ� �ִ���
        {
            Vector3 direction = player.position - transform.position + Vector3.up; // ���� ����
            Ray ray = new Ray(transform.position, direction); // ray ����
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
