using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    //�׻� �ν����� Ȯ���غ� ��.
    [SerializeField] float interactionRadius;

    IinteractableObj obj;
    PlayerStatus _status;

    private void Start()
    {
        _status = this.GetComponent<PlayerStatus>();

        GameManager.Input.InputDelegate += ObjFounding;
        GameManager.Input.InputDelegate += WorkingObj;
    }

    void ObjFounding()
    {
        if (_status.IsAlive == false) { return; }
        Collider[] temps = Physics.OverlapSphere(this.transform.position, interactionRadius);

        foreach (Collider item in temps)
        {
            if (item.TryGetComponent<IinteractableObj>(out obj))
            {
                break;
            }
            else
            {
                obj = null;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(this.transform.position, interactionRadius);
    }
    void WorkingObj()
    {
        if (_status.IsAlive == false) { return; }
        //�Է� ���ų� ��ȣ�ۿ��� ������Ʈ ������ ����
        if (GameManager.Input.InteractionTrigger == false || obj == null) { return; }

        obj.Interaction();

        if (((MonoBehaviour)obj).GetComponent<EnemyInteractive>() != null && _status.excuting == false)
        {
            _status.excuting = true;
            StartCoroutine(Excuting());
        }
    }

    IEnumerator Excuting()
    {
        yield return new WaitForSeconds(3.2f);
        _status.excuting = false;
    }
}
