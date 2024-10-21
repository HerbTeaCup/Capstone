using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    //항상 인스펙터 확인해볼 것.
    [SerializeField] float interactionRadius;
    [SerializeField] [Range(0.5f,1.5f)] float upperOffset;

    IinteractableObj obj;
    PlayerStatus _status;
    private Camera mainCamera;

    private void Start()
    {
        _status = this.GetComponent<PlayerStatus>();
        mainCamera = Camera.main;

        GameManager.Input.InputDelegate += ObjFounding;
        GameManager.Input.InputDelegate += WorkingObj;
    }

    void ObjFounding()
    {
        if (_status.IsAlive == false) { return; }
        Collider[] temps = Physics.OverlapSphere(this.transform.position + Vector3.up * upperOffset, interactionRadius);

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

        if (obj != null)
        {
            obj.ui_Show = true;
            obj.UpdateUIPosition(mainCamera);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(this.transform.position + Vector3.up * upperOffset, interactionRadius);
    }
    void WorkingObj()
    {
        if (_status.IsAlive == false) { return; }
        //입력 없거나 상호작용할 오브젝트 없으면 리턴
        if (GameManager.Input.InteractionTrigger == false || obj == null) { return; }

        obj.Interaction();
        EnemyInteractive enemy;

        if (((MonoBehaviour)obj).TryGetComponent<EnemyInteractive>(out enemy) && _status.excuting == false)
        {
            if (enemy._status.executable == false)
            {
                return;
            }

            _status.excuting = true;
            StartCoroutine(Excuting());
        }
    }

    IEnumerator Excuting()
    {
        _status.ExcuteTransform = ((MonoBehaviour)obj).transform;
        yield return new WaitForSeconds(2.0f);
        _status.ExcuteTransform = this.transform;
        yield return new WaitForSeconds(1.2f);
        _status.excuting = false;
    }

}
