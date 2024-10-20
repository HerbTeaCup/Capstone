using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    EnemyStatus _status;
    Animator _anim;

    bool _dead = false;
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
        if (_status.IsAlive == false && _dead == false)
        {
            _anim.SetTrigger("Dying");
            _dead = true;
        }

        _anim.SetFloat("Speed", _status.currnetSpeed);
    }

    void Excute()
    {
        if(_status.executing == false) { return; }

        _anim.applyRootMotion = true;
        _anim.SetBool("Excute", true);
    }
}
