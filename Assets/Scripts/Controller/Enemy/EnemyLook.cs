using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLook : MonoBehaviour
{
    EnemyStatus _status;

    bool _gizmoColor = false;

    float _timeDelta = 0f;
    // Start is called before the first frame update
    void Start()
    {
        _status = GetComponent<EnemyStatus>();

        GameManager.Enemy.UpdateDelegate += StateUpdate;
    }

    bool Searching(out float distanceToPlayer)
    {
        Collider[] temp = Physics.OverlapSphere(this.transform.position, _status.searchRadius, LayerMask.GetMask("Player"));
        RaycastHit hit;

        distanceToPlayer = Mathf.Infinity;
        _gizmoColor = temp.Length > 0;

        if (temp.Length == 0)
            return false;

        Debug.DrawLine(this.transform.position + Vector3.up * 1.5f, temp[0].transform.position + Vector3.up * 1.5f);

        Vector3 directionToTarget = (temp[0].transform.position - this.transform.position).normalized;

        if (!Physics.Raycast(this.transform.position + Vector3.up * 1.5f, directionToTarget, out hit, _status.searchRadius))
            return false;
        if (hit.transform.gameObject.layer != LayerMask.NameToLayer("Player"))
            return false; // ���̾ Player�� �ƴϸ� false ��ȯ

        distanceToPlayer = Vector3.Distance(this.transform.position, temp[0].transform.position);
        return Vector3.Dot(directionToTarget, this.transform.forward) > 0.596f;
    }
    void StateUpdate()
    {
        float distanceToPlayer;
        bool foundPlayer = Searching(out distanceToPlayer);

        _status.currentTime = _timeDelta;
        if (foundPlayer)
        {
            // �Ÿ��� �������� Ž�� �ӵ� ����
            float detectionSpeed = _status.searchRadius / Mathf.Max(distanceToPlayer, 0.1f);
            _timeDelta += Time.deltaTime * detectionSpeed;

            //�߰� ���°� �Ǹ� 7�ʰ� �Ǽ� 2���� �����ð��� ��
            if (_timeDelta > 7f || _status.state == EnemyState.Capture) { _timeDelta = 7f; return; }
        }
        else
        {
            _timeDelta = Mathf.Max(0, _timeDelta - Time.deltaTime);
            if(_timeDelta < 0.1f) { _timeDelta = 0f; }
        }

        if (_timeDelta > _status.captureTime)
        {
            _status.state = EnemyState.Capture;
        }
        else if (_timeDelta > _status.boundaryTime)
        {
            _status.state = EnemyState.Boundary;
        }
        else
        {
            _status.state = EnemyState.Idle;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmoColor ? Color.green : Color.red;

        Gizmos.DrawWireSphere(this.transform.position, 10f);
    }
}
