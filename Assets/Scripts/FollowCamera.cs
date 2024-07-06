using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] GameObject target; 
    [SerializeField] Vector3 offset; // camera는 target 기준으로 얼마나 떨어져있나

    void LateUpdate()
    {
        transform.position = target.transform.position + offset;
        transform.LookAt(target.transform); // camera가 target을 바라봄
    }
}
