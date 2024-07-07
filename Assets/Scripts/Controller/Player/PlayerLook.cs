using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    PlayerStatus _status;

    float axisY = 0f;

    [SerializeField] Transform CameraArm;
    // Start is called before the first frame update
    void Start()
    {
        _status = GetComponent<PlayerStatus>();

        GameManager.Input.InputDelegate += Turning;
    }

    void Turning()
    {
        axisY += GameManager.Input.ViewMove * _status.viewMaxSpeed * Time.deltaTime;

        CameraArm.rotation = Quaternion.Euler(0, axisY, 0);
    }
}
