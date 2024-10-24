using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    EnemyStatus _status;
    Animator _anim;

    bool wasHitting = false;  // 이전 피격 상태 저장

    // Start is called before the first frame update
    void Start()
    {
        _status = GetComponent<EnemyStatus>();
        _anim = GetComponent<Animator>();

        GameManager.Enemy.UpdateDelegate += Excute;
    }
    private void Update()
    {
        UpdateParameter();
    }

    void UpdateParameter()
    {
        _anim.SetBool("Dying", !_status.IsAlive);

        ApplyHitAnimation();

        wasHitting = _status.hitting;  // 현재 피격 상태를 저장

        _anim.SetBool("Chase", _status.state == EnemyState.Capture);
        _anim.SetFloat("Speed", _status.currnetSpeed);
    }

    void ApplyHitAnimation()
    {
        if (_status.hitting == false || wasHitting)
            return;
        if (_status.player == null)
        {
            _anim.SetFloat("HitX", 0);
            _anim.SetFloat("HitY", 0);
            _anim.SetTrigger("HitTrigger");
            return;
        }

        Vector3 hitDirection = (this._status.player.position - transform.position).normalized;
        float angle = Vector3.SignedAngle(transform.forward, hitDirection, Vector3.up);

        // 피격 각도를 X, Y 파라미터로 변환
        float hitX = Mathf.Sin(Mathf.Deg2Rad * angle);
        float hitY = Mathf.Cos(Mathf.Deg2Rad * angle);

        // BlendTree의 파라미터에 적용
        _anim.SetFloat("HitX", hitX);
        _anim.SetFloat("HitY", hitY);

        // 피격 애니메이션 트리거
        _anim.SetTrigger("HitTrigger");
    }


    void Excute()
    {
        if(_status.executing == false) { return; }

        _anim.applyRootMotion = true;
        _anim.SetBool("Excute", true);
    }
}
