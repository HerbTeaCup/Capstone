using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : IManager
{
    public System.Action UpdateDelegate = null;
    public System.Action LateDelegate = null;

    public void Updater()
    {
        if (UpdateDelegate != null) { UpdateDelegate(); }
    }
    public void LateUpdater()
    {
        if (LateDelegate != null) { LateDelegate(); }
    }

    public void Clear()
    {
        UpdateDelegate = null;
        LateDelegate = null;
    }
}
