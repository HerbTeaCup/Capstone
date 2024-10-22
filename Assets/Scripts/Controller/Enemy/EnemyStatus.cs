using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : GenericUnit
{
    [Header("EnemeyStatus")]
    public EnemySound Sound = null;
    public Transform player = null;
    public Transform trapTransform = null;
    public EnemyState state = EnemyState.Idle;

    public bool isReloading = false; //재장전
    public bool curveNeed = false; //추격중 회전에 필요한 변수
    public bool attraction = false; //true면 유인 당하는 중
    public bool executable = false; //true 일 때만 플레이어가 처형 가능
    public bool executing = false; //true면 처형당하는 중
    public bool hitting = false;

    public bool weakDetecting = false;
    public bool strongDetecting = false;

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

        Sound = GetComponent<EnemySound>();

        this.walkTemp = this.walkSpeed;
        this.runTemp = this.runSpeed;
    }
    private void Update()
    {
        if (IsAlive == false)
        {
            Destroy(GetComponent<CapsuleCollider>());
            Destroy(GetComponent<Rigidbody>());
        }
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
        this.hitting = true;

        yield return new WaitForSeconds(1.8f);
        if (player != null)
        {
            Vector3 dir = (player.position - this.transform.position).normalized;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(dir), 5f * Time.deltaTime);
        }

        this.hitting = false;
        this.walkSpeed = walkTemp;
        this.runSpeed = runTemp;
    }
}
