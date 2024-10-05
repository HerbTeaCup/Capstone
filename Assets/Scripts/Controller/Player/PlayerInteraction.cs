using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    //항상 인스펙터 확인해볼 것.
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
        //입력 없거나 상호작용할 오브젝트 없으면 리턴
        if (GameManager.Input.InteractionTrigger == false || obj == null) { return; }

        obj.Interaction();
    }
}
