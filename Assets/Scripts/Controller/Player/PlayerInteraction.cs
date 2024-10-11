using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    //항상 인스펙터 확인해볼 것.
    [SerializeField] float interactionRadius; // 상호작용 반경

    [Header("Stealth UI")]
    [SerializeField] GameObject stealthUI;
    [SerializeField] Vector3 uiOffset = new Vector3(0, 2f, 0); // 머리 위로 UI를 올리기 위한 오프셋 값

    IinteractableObj obj;
    PlayerStatus _status;

    private void Start()
    {
        _status = this.GetComponent<PlayerStatus>();

        GameManager.Input.InputDelegate += ObjFounding;
        GameManager.Input.InputDelegate += WorkingObj;
    }

    // 상호작용 가능한 오브젝트 서치
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

        //입력 없거나 상호작용할 오브젝트 없으면 리턴
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
            //예시
            ui.SetActive(false);
            return;
        }

        //밑에서부터 작업시작
        Debug.Log($"UIShow Logic is Working");
        //예시
        ui.SetActive(true);
    }

    // 스텔스 UI를 머리 위로 이동시키는 함수
    void UpdateUIPosition(GameObject ui, IinteractableObj obj)
    {
        if (ui != null)
        {
            ui.transform.position = ((MonoBehaviour)obj).transform.position + uiOffset;
        }
    }

}
