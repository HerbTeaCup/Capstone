using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : GenericUnit
{
    public EnemyState state = EnemyState.Idle;

    public float searchRadius = 10f;
    public float boundaryTime = 2f;
    public float captureTime = 5f;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }
}
