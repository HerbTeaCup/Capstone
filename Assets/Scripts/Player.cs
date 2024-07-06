using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Player : MonoBehaviour
{
    public float speed;
    float h, v;
    bool wDown;

    Vector3 moveVec;

    Animator ani;
    void Awake()
    {
        ani = GetComponentInChildren<Animator>();
    }
    void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        wDown = Input.GetButton("Walk");

        moveVec = new Vector3(h, 0, v).normalized;
        transform.position += moveVec * speed * Time.deltaTime;

        if (wDown)
            transform.position += moveVec * speed * 0.3f * Time.deltaTime;
        else
            transform.position += moveVec * speed * Time.deltaTime;

        ani.SetBool("IsRunning", moveVec != Vector3.zero);
        ani.SetBool("IsWalking", wDown);

        transform.LookAt(transform.position + moveVec);
    }
}
