using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class CamRotate : MonoBehaviour
{
    public float rotateSpeed = 200f;

    float mx = 0;
    float my = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float mouse_X = Input.GetAxis("Mouse X");
        float mouse_Y = Input.GetAxis("Mouse Y");

        mx += mouse_X * rotateSpeed * Time.deltaTime;
        my += mouse_Y * rotateSpeed * Time.deltaTime;

        my = Mathf.Clamp(my, -90f, 90f);
        
        transform.eulerAngles = new Vector3(-mx, my, 0);
    }
}
