using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    //�׻� �ν����� Ȯ���غ� ��.
    [SerializeField] float interactionRadius;

    IinteractableObj obj;

    private void Start()
    {
        GameManager.Input.InputDelegate += ObjFounding;
        GameManager.Input.InputDelegate += WorkingObj;
    }

    void ObjFounding()
    {
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
        //�Է� ���ų� ��ȣ�ۿ��� ������Ʈ ������ ����
        if (GameManager.Input.InteractionTrigger == false || obj == null) { return; }

        obj.Interaction();
    }
}
