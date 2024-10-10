using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObjExtand : MonoBehaviour, IinteractableObj
{
    public bool interactable { get; set; } = true;
    public bool calling { get; protected set; } = false; //true가 되면 적을 부름
    public bool ui_Show { get; set; }

    public virtual void Interaction()
    {

    }

    public virtual void ShowInteractionUI(GameObject UI)
    {
        if (UI != null)
        {
            UI.SetActive(ui_Show); // ui_Show 플래그에 따라 UI 표시/숨김
        }
    }
}
