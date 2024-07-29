using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : GenericUnit
{
    [Header("EnemeyStatus")]
    public EnemyState state = EnemyState.Idle;

    public float searchRadius = 10f;
    public float boundaryTime = 2f;
    public float captureTime = 5f;
    public float currentTime;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }
}
