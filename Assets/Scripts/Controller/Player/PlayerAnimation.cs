using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    PlayerStatus _status;

    Animator _anim;
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
