using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    PlayerStatus _status;

    Animator _anim;

    bool _shootDelta = false;
    bool _isExecuting = false;
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

        _anim.SetFloat("Speed", _status.currnetSpeed);
        _anim.SetBool("Aiming", GameManager.Input.Aiming);
        _anim.SetBool("ShootDelta", _shootDelta);
    }
    void InputParameterUpdate()
    {
        if (_status.CurrentWeapon.AmmoMax > 0 && GameManager.Input.FireTrigger && GameManager.Input.Aiming && _shootDelta == false)
        {
            _shootDelta = true;
            _anim.SetTrigger("Shoot");
        }

        if (_status.excuting && !_isExecuting) // �̹� ���� ���� �ƴ� ��쿡�� Ʈ����
        {
            _anim.SetTrigger("Excute");
            _isExecuting = true; // �ִϸ��̼� ���۵��� ǥ��
        }
        if (!_status.excuting && _isExecuting)
        {
            _isExecuting = false; // ������ excute�� �ٽ� ������ �� �ֵ��� �÷��� �ʱ�ȭ
        }

        if (_status.CurrentWeapon.fireCurrentRate <= 0f)
        {
            _shootDelta = false;
            return;
        }
    }
}
