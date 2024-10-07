using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : GenericUnit
{
    [Header("EnemeyStatus")]
    public Transform player = null;
    public Transform trapTransform = null;
    public EnemyState state = EnemyState.Idle;

    public bool isReloading = false; //재장전
    public bool curveNeed = false; //추격중 회전에 필요한 변수
    public bool attraction = false; //true면 유인 당하는 중
    public bool executable = false; //true 일 때만 플레이어가 처형 가능
    public bool executing = false; //true면 처형당하는 중

    public float searchRadius = 10f;
    public float boundaryTime = 2f;
    public float captureTime = 5f;
    public float currentTime;

    float walkTemp;
    float runTemp;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        this.walkTemp = this.walkSpeed;
        this.runTemp = this.runSpeed;

        GameManager.Stage.remainingEnemy++;
    }

    public override void TakeDamage(int dmg)
    {
        base.TakeDamage(dmg);
        StopCoroutine(Getstiff());
        StartCoroutine(Getstiff());
    }

    IEnumerator Getstiff()
    {
        this.walkSpeed = 0f;
        this.runSpeed = 0f;
        yield return new WaitForSeconds(0.6f);
        this.walkSpeed = walkTemp;
        this.runSpeed = runTemp;
    }
}
