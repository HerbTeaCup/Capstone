using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] Transform CameraArm;
    [SerializeField] Transform TargetPos;

    CharacterController _cc;
    PlayerStatus _status;

    Vector3 _Dir = new Vector3(0, 0, 0);
    Vector3 _targetDir = new Vector3();

    float _speed = 0f;
    float _verticalSpeed = 0f;
    float _gravity = -9.8f;

    float _targetRotation = 0f;

    float _speedOffset = 0.01f;
    float _radius = 0.28f;

    bool _gizmoColor = false;

    // Start is called before the first frame update
    void Start()
    {
        _cc = GetComponent<CharacterController>();
        _status = GetComponent<PlayerStatus>();

        GameManager.Input.InputDelegate += GroundCheck;
        GameManager.Input.InputDelegate += WorldMove;
        GameManager.Input.InputDelegate += Gravity;
        GameManager.Input.InputDelegate += Rotate;

        _radius = _cc.radius;
    }

    void WorldMove()
    {
        float targetSpeed = GameManager.Input.Sprint ? _status.runSpeed : _status.walkSpeed;

        if (GameManager.Input.XZdir == Vector2.zero) { targetSpeed = 0f; }

        if (_status.currnetSpeed < targetSpeed - _speedOffset || _status.currnetSpeed > targetSpeed + _speedOffset)
        {
            _speed = Mathf.Lerp(_speed, targetSpeed, _status.speedBlend * Time.deltaTime);
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }

        if (GameManager.Input.XZdir != Vector2.zero)
        {
            _Dir.x = GameManager.Input.XZdir.x;
            _Dir.z = GameManager.Input.XZdir.y;
        }

        _cc.Move(_Dir * _speed * Time.deltaTime + new Vector3(0, _verticalSpeed, 0) * Time.deltaTime);
        _status.currnetSpeed = _speed;
    }
    void RelativeMove()
    {
        //카메라 기준 이동 메소드
        //카메라를 회전시킬 때 유용하나 어째서인지 굉장히 버벅거리는 현상 있음
        float targetSpeed = GameManager.Input.Sprint ? _status.runSpeed : _status.walkSpeed;

        if(GameManager.Input.XZdir == Vector2.zero) { targetSpeed = 0f; }

        if (_status.currnetSpeed < targetSpeed - _speedOffset || _status.currnetSpeed > targetSpeed + _speedOffset)
        {
            _speed = Mathf.Lerp(_speed, targetSpeed, _status.speedBlend * Time.deltaTime);
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }

        if (GameManager.Input.XZdir != Vector2.zero)
        {
            _Dir.x = GameManager.Input.XZdir.x;
            _Dir.z = GameManager.Input.XZdir.y;

            _targetRotation = Mathf.Atan2(_Dir.x, _Dir.z) * Mathf.Rad2Deg + CameraArm.transform.eulerAngles.y;
        }

        _targetDir = (Quaternion.Euler(0, _targetRotation, 0) * Vector3.forward).normalized;

        _cc.Move(_targetDir * _speed * Time.deltaTime + new Vector3(0, _verticalSpeed, 0) * Time.deltaTime);
        _status.currnetSpeed = _speed;
    }
    void Rotate()
    {
        if (GameManager.Input.Aiming)
        {
            this.transform.LookAt(new Vector3(TargetPos.position.x, this.transform.position.y, TargetPos.position.z));
        }

        if (GameManager.Input.XZdir == Vector2.zero) { return; }

        if (GameManager.Input.Aiming) { return; }
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(_Dir),
            _status.speedBlend * Time.deltaTime);
    }
    void Gravity()
    {
        if (_status.isGrounded)
        {
            if (_verticalSpeed < 0f)
            {
                _verticalSpeed = -2f;
            }
        }
        else
        {
            _verticalSpeed += _gravity * Time.deltaTime;
        }
    }
    #region GroundCheck
    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmoColor ? Color.green : Color.red;
        Vector3 checkPosition = this.transform.position + new Vector3(0, _radius - 0.05f, 0);

        Gizmos.DrawSphere(checkPosition, _radius);
    }
    void GroundCheck()
    {
        Vector3 checkPosition = this.transform.position + new Vector3(0, _radius - 0.05f, 0);

        _status.isGrounded = Physics.CheckSphere(checkPosition, _radius, _status.GroundLayer);
        _gizmoColor = _status.isGrounded;
    }
    #endregion
}
