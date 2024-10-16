using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : IManager
{
    public int detectionDegree = 0;
    public int remainingEnemy = 0;
    public bool targetEliminated = false;

    public void Clear()
    {
        detectionDegree = 0;
        remainingEnemy = 0;
        targetEliminated = false;
    }
}
