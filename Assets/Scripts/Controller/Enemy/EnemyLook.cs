using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLook : MonoBehaviour
{
    EnemyStatus _status;

    bool _gizmoColor = false;

    float _timeDelta = 0f;
    [SerializeField] float detectionRate;

    // Start is called before the first frame update
    void Start()
    {
        _status = GetComponent<EnemyStatus>();

        GameManager.Enemy.UpdateDelegate += StateUpdate;
    }

    bool Searching(out float distanceToPlayer)
    {
        Collider[] temp = Physics.OverlapSphere(this.transform.position, _status.searchRadius, LayerMask.GetMask("Player", "Trap"));
        RaycastHit hit;
        
        distanceToPlayer = Mathf.Infinity;
        _gizmoColor = temp.Length > 0;

        if (temp.Length == 0)
            return false;

        //�������� ���� ���� ������Ʈ�� ���� �� �����Ƿ� �����迭�� ����
        List<IinteractableObj> interactiveObjArray = new List<IinteractableObj>();

        foreach (Collider item in temp)
        {
            IinteractableObj objTemp = item.GetComponent<IinteractableObj>();
            if (objTemp != null)
            {
                interactiveObjArray.Add(objTemp);
                continue; //Player�鼭 ��ȣ�ۿ�������� ���� �����Ƿ� null �ƴϸ� �ٷ� break
            }

            //�Ʒ����ʹ� obj�� �ƴ϶�� ���� Ȯ���ϹǷ�
            _status.player = item.transform;
        }

        //false�� �����ص� ������ �����ϸ� �˾Ƽ� true�� �ٲ�
        _status._attraction = false;
        foreach (IinteractableObj item in interactiveObjArray)
        {
            //���� �迭�� ���ٸ� ������� ���� ��
            if (item.calling)
                _status._attraction = true;
        }

        //�÷��̾ �������� ������ ��ã���ϱ� false ��ȯ
        if(_status.player == null) { return false; }
        //������ ���� ���� ���ϱ�
        Vector3 directionToTarget = (_status.player.position - this.transform.position).normalized;

        //�����ؼ� ���� ���
        if (Vector3.Dot(directionToTarget, this.transform.forward) < 0.4f)
            return false;
        //������ ����ϸ� Raycast�ؼ� ���̿� ������ ������Ʈ �ִ��� üũ
        if (Physics.Raycast(this.transform.position + Vector3.up * 1.5f, directionToTarget, out hit, _status.searchRadius) == false)
            return false;

        //���ƾ� �ϴ��� üũ
        CurvedCheck(directionToTarget);

        Debug.DrawLine(this.transform.position + Vector3.up * 1.5f, _status.player.position + Vector3.up * 1.5f);

        if (hit.transform.gameObject.layer != LayerMask.NameToLayer("Player"))
            return false; // ���̾ Player�� �ƴϸ� false ��ȯ

        distanceToPlayer = Vector3.Distance(this.transform.position, _status.player.position);

        //��� ������ ��������Ƿ� true ��ȯ
        return true;
    }
    void StateUpdate()
    {
        //�÷��̾ �����ϰ� ���¸� �ٲٴ� �޼ҵ�
        float distanceToPlayer;
        bool foundPlayer = Searching(out distanceToPlayer);
        
        _status.currentTime = _timeDelta;
        if (foundPlayer)
        {
            // �Ÿ��� �������� Ž�� �ӵ� ����
            float detectionSpeed = _status.searchRadius / Mathf.Max(distanceToPlayer, 0.1f) * detectionRate;
            _timeDelta += Time.deltaTime * detectionSpeed;

            //�߰� ���°� �Ǹ� 10�ʰ� �Ǽ� 5���� �����ð��� ��
            if (_timeDelta > 10f || _status.state == EnemyState.Capture) { _timeDelta = 10f; return; }
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
    void CurvedCheck(Vector3 dir)
    {
        if (_status.state != EnemyState.Capture)
            return;

        if (Physics.Raycast(this.transform.position + Vector3.up, dir, _status.searchRadius, 1 << 8))
        {
            _status.curveNeed = true;
        }
        else
        {
            _status.curveNeed = false;
        }
        Debug.Log($"Raycast =  {_status.curveNeed}");
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmoColor ? Color.green : Color.red;

        Gizmos.DrawWireSphere(this.transform.position, 10f);
    }
}
