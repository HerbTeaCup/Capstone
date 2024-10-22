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

    public bool isReloading = false; //������
    public bool curveNeed = false; //�߰��� ȸ���� �ʿ��� ����
    public bool attraction = false; //true�� ���� ���ϴ� ��
    public bool executable = false; //true �� ���� �÷��̾ ó�� ����
    public bool executing = false; //true�� ó�����ϴ� ��
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
