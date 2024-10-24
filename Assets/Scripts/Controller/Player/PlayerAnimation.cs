using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    PlayerStatus _status;

    Animator _anim;

    bool _shootDelta = false;
    bool _closeAttackDelta = false;
    bool _isExecuting = false;
    bool _Dying = false;

    // Start is called before the first frame update
    void Start()
    {
        _status = GetComponent<PlayerStatus>();
        _anim = GetComponent<Animator>();

        GameManager.Input.InputDelegate += ParameterUpdate;
        GameManager.Input.InputDelegate += InputParameterUpdate;
    }

    void ParameterUpdate()
    {
        _anim.applyRootMotion = _status.excuting;

        if (_status.IsAlive == false && _Dying == false)
        {
            _anim.SetBool("Dying", true);
            _anim.applyRootMotion = true;
            _Dying = true;
            return;
        }
        else
        {
            _anim.SetBool("Dying", false);
        }

        _anim.SetFloat("Speed", _status.currnetSpeed);
        _anim.SetBool("Aiming", GameManager.Input.Aiming);
        _anim.SetBool("ShootDelta", _shootDelta);
        _anim.SetBool("CloseDelta", _closeAttackDelta);
    }
    void InputParameterUpdate()
    {
        if (_status.CurrentWeapon.Magazine > 0 && GameManager.Input.FireTrigger && GameManager.Input.Aiming && _shootDelta == false && _status.isReloading == false)
        {
            _shootDelta = true;
            _anim.SetTrigger("Shoot");
        }
        if (_status.excuting && !_isExecuting) // 이미 실행 중이 아닌 경우에만 트리거
        {
            _anim.SetTrigger("Excute");
            _isExecuting = true; // 애니메이션 시작됨을 표시
        }
        if (!_status.excuting && _isExecuting)
        {
            _isExecuting = false; // 다음에 excute를 다시 실행할 수 있도록 플래그 초기화
        }
        if (GameManager.Input.Aiming == false && GameManager.Input.FireTrigger && _closeAttackDelta == false)
        {
            _closeAttackDelta = true;
            _anim.SetTrigger("CloseAttack");
        }

        if (_status.CurrentWeapon.fireCurrentRate <= 0f)
        {
            _shootDelta = false;
        }

        if (_status.isMoveable)
        {
            _closeAttackDelta = false; // 이동 가능할 때만 근접 공격 플래그 초기화
        }
    }
}
