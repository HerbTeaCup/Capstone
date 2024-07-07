using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour, IManager
{
    public System.Action InputDelegate = null;

    public Vector2 XZdir { get; private set; }
    public bool FireTrigger { get; private set; }
    public bool Aiming { get; private set; }
    public bool Sprint { get; private set; }
    public float ViewMove { get; private set; }

    public void Updater()
    {
        if(InputDelegate != null) { InputDelegate(); }

        ParameterUpdate();
    }

    public void Clear() //IManager Interface
    {
        //모든 체인 해제
        InputDelegate = null;
    }

    void ParameterUpdate()
    {
        if (FireTrigger == true) { FireTrigger = false; } //bool 변수 트리거화
    }

    //Action Binding
    public void OnMove(InputAction.CallbackContext context)
    {
        if(context.started) { return; }
        XZdir = context.ReadValue<Vector2>();
    }
    public void OnFire(InputAction.CallbackContext context)
    {
        if(context.performed == false) { FireTrigger = false; return; }//입력이 인정되지 않으면 리턴
        FireTrigger = true;
    }
    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.performed) { Aiming = true; }
        else if (context.canceled) { Aiming = false; }
    }
    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed) { Sprint = true; }
        else if (context.canceled) { Sprint = false; }
    }
    public void OnViewMovement(InputAction.CallbackContext context)
    {
        if(context.performed) 
        {
            //입력 중일 때만 작동
            ViewMove = context.ReadValue<float>(); 
        }
        else if (context.canceled) { ViewMove = 0f; }
    }
}
