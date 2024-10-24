using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] Transform CameraArm;
    [SerializeField] Transform TargetPos;
    [SerializeField] float offsetDistance = 0.3f;

    CharacterController _cc;
    PlayerStatus _status;

    Vector3 _Dir = Vector3.zero;
    Vector3 _targetDir = Vector3.zero;

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

        //GameManager.Input.InputDelegate += GroundCheck;
        //GameManager.Input.InputDelegate += RelativeMove;
        //GameManager.Input.InputDelegate += Gravity;
        //GameManager.Input.InputDelegate += Rotate;
        //GameManager.Input.InputDelegate += WorldMove;

        _radius = _cc.radius;

        _targetRotation = this.transform.rotation.eulerAngles.y;
    }
    private void Update()
    {
        GroundCheck();
        RelativeMove();
        Gravity();
        ExcuteTransformMove(_status.ExcuteTransform);
    }

    void RelativeMove()
    {
        if (_status.IsAlive == false || _status.excuting) { return; }

        float targetSpeed = GameManager.Input.Sprint ? _status.runSpeed : _status.walkSpeed;

        if(GameManager.Input.XZdir == Vector2.zero || GameManager.Input.Aiming || _status.isMoveable == false) { targetSpeed = 0f; }

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
        if (GameManager.Input.Aiming)
        {
            this.transform.rotation = Quaternion.LookRotation(this.transform.forward);

            _targetRotation = this.transform.rotation.eulerAngles.y;
            
        }

        _targetDir = (Quaternion.Euler(0, _targetRotation, 0) * Vector3.forward).normalized;
        _status.TargetDir = _targetDir;

        _cc.Move(_targetDir * _speed * Time.deltaTime + new Vector3(0, _verticalSpeed, 0) * Time.deltaTime);
        _status.currnetSpeed = _speed;

        if (GameManager.Input.Aiming)
        {
            this.transform.LookAt(new Vector3(TargetPos.position.x, this.transform.position.y, TargetPos.position.z));
        }
        else if(_status.excuting == false && _status.isMoveable == true)
        {
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.Euler(0, _targetRotation, 0),
                _status.trunSpeedBlend * Time.deltaTime);
        }
    }
    void ExcuteTransformMove(Transform targetTrans)
    {
        if (_status.excuting == false)
            return;
        if (_status.ExcuteTransform == null)
        {
            Debug.Log("ExcuteTransform is null");
            return;
        }

        Vector3 targetPosition = targetTrans.position - targetTrans.forward * offsetDistance;

        // 타겟 위치와 현재 위치의 차이 벡터 계산
        Vector3 moveDirection = targetPosition - this.transform.position;

        // 시간 기반으로 이동 처리
        _cc.Move(moveDirection * Time.deltaTime);
    }
    void Gravity()
    {
        if (_status.IsAlive == false) { return; }
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
