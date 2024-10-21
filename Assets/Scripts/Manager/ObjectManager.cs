using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public delegate void UpdateAction();
    public event UpdateAction UpdateDelegate;

    public void Updater()
    {
        if (UpdateDelegate != null)
        {
            UpdateDelegate.Invoke();
        }
    }

    public void Clear()
    {

    }
}
