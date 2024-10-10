using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    //�׻� �ν����� Ȯ���غ� ��.
    [SerializeField] float interactionRadius; // ��ȣ�ۿ� �ݰ�

    IinteractableObj obj;
    PlayerStatus _status;

    [SerializeField] GameObject stealthUI;

    private void Start()
    {
        _status = this.GetComponent<PlayerStatus>();

        GameManager.Input.InputDelegate += ObjFounding;
        GameManager.Input.InputDelegate += WorkingObj;
    }

    // ��ȣ�ۿ� ������ ������Ʈ ��ġ
    void ObjFounding()
    {
        if (_status.IsAlive == false) { return; }
        Collider[] temps = Physics.OverlapSphere(this.transform.position, interactionRadius);

        foreach (Collider item in temps)
        {
            if (item.TryGetComponent<IinteractableObj>(out obj))
            {
                if (obj is EnemyInteractive)
                {
                    obj.ui_Show = true;
                    obj.ShowInteractionUI(stealthUI);

                    break;
                }
            }

            // ��ȣ�ۿ��� ������Ʈ�� ������ UI ����
            if (obj == null || !(obj is EnemyInteractive))
            {
                stealthUI.SetActive(false); // UI �����
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
        obj.ui_Show = false;
        obj.ShowInteractionUI(stealthUI);

        if (((MonoBehaviour)obj).GetComponent<EnemyInteractive>() != null && _status.excuting == false)
        {
            _status.excuting = true;
            _status.ExcuteTransform = ((MonoBehaviour)obj).transform;

            StartCoroutine(Excuting());
        }
    }

    IEnumerator Excuting()
    {
        yield return new WaitForSeconds(3.2f);
        _status.excuting = false;
    }
}
