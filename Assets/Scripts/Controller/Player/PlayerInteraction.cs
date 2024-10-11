using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    //�׻� �ν����� Ȯ���غ� ��.
    [SerializeField] float interactionRadius; // ��ȣ�ۿ� �ݰ�

    [Header("Stealth UI")]
    [SerializeField] GameObject stealthUI;
    [SerializeField] Vector3 uiOffset = new Vector3(0, 2f, 0); // �Ӹ� ���� UI�� �ø��� ���� ������ ��

    IinteractableObj obj;
    PlayerStatus _status;

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
                    UpdateUIPosition(stealthUI, obj);
                    UIShow(stealthUI, obj);
                }

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
            _status.ExcuteTransform = ((MonoBehaviour)obj).transform;

            StartCoroutine(Excuting());
        }
    }

    IEnumerator Excuting()
    {
        yield return new WaitForSeconds(3.2f);
        _status.excuting = false;
    }

    void UIShow(GameObject ui, IinteractableObj obj)
    {
        if (obj.ui_Show == false)
        {
            //����
            ui.SetActive(false);
            return;
        }

        //�ؿ������� �۾�����
        Debug.Log($"UIShow Logic is Working");
        //����
        ui.SetActive(true);
    }

    // ���ڽ� UI�� �Ӹ� ���� �̵���Ű�� �Լ�
    void UpdateUIPosition(GameObject ui, IinteractableObj obj)
    {
        if (ui != null)
        {
            ui.transform.position = ((MonoBehaviour)obj).transform.position + uiOffset;
        }
    }

}
