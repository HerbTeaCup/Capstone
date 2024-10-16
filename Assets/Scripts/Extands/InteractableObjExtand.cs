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

    public virtual void UpdateUIPosition(Camera camera)
    {

    }
}
