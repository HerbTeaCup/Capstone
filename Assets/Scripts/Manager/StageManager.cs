using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : IManager
{
    public int detectionDegree = 0;
    public int remainingStageObj = 0;
    public bool targetEliminated = false;
    public bool Clearable = false;

    public void Clear()
    {
        detectionDegree = 0;
        remainingStageObj = 0;
        targetEliminated = false;
        Clearable = false;
    }
}
