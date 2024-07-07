using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookIsel : MonoBehaviour
{
    [SerializeField] Transform CameraArm;
    PlayerStatus _status;

    //camera
    float _targetYaw = 0f;
    float _targetPitch = 0f;

    [Header("View")]
    [SerializeField] [Range(0, 0.5f)] float mouseSensitive;
    [SerializeField] float bottomClampAngle, topClampAngle;
    [Header("Target Found")]
    [SerializeField] Collider[] targets;

    // Start is called before the first frame update
    void Start()
    {
        _status = GetComponent<PlayerStatus>();

        GameManager.Input.InputDelegate += CameraRotate;
    }

    void CameraRotate()
    {
        //Vector2 mouseMove = GameManager.Input.MouseMove;

        //if (mouseMove.sqrMagnitude > 0.01f)
        //{
        //    _targetPitch += mouseMove.x * mouseSensitive;
        //    _targetYaw += mouseMove.y * mouseSensitive;
        //}

        _targetPitch = AngleClamp(float.MinValue, float.MaxValue, _targetPitch);
        _targetYaw = AngleClamp(bottomClampAngle, topClampAngle, _targetYaw);

        CameraArm.rotation = Quaternion.Euler(_targetYaw, _targetPitch, 0);
    }
    float AngleClamp(float min, float max, float value)
    {
        if (value < -360) value += 360;
        if (value > 360) value -= 360;

        return Mathf.Clamp(value, min, max);
    }
}
