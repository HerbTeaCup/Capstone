using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    PlayerStatus _status;

    Animator _anim;

    bool _shootDelta = false;
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

        if (_status.CurrentWeapon.fireCurrentRate <= 0f)
        {
            _shootDelta = false;
            return;
        }
    }
}
