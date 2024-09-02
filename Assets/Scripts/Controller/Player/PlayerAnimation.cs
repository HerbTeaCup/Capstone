using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    PlayerStatus _status;

    Animator _anim;

    float _horizon = 0f;
    float _vertical = 0f;

    float _lerpRatio = 0.4f;
    // Start is called before the first frame update
    void Start()
    {
        _status = GetComponent<PlayerStatus>();
        _anim = GetComponent<Animator>();

        GameManager.Input.InputDelegate += ParameterUpdate;
    }

    void ParameterUpdate()
    {
        _anim.SetFloat("Speed", _status.currnetSpeed);
        _anim.SetBool("Aiming", GameManager.Input.Aiming);
    }
}
