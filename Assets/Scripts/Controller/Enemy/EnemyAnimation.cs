using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    EnemyStatus _status;
    Animator _anim;
    // Start is called before the first frame update
    void Start()
    {
        _status = GetComponent<EnemyStatus>();
        _anim = GetComponent<Animator>();

        GameManager.Enemy.UpdateDelegate += Excute;
    }

    void Excute()
    {
        if(_status.executing == false) { return; }

        _anim.applyRootMotion = true;
        _anim.SetBool("Excute", true);
    }
}
