using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] GameObject target; 
    [SerializeField] Vector3 offset; // camera�� target �������� �󸶳� �������ֳ�

    void LateUpdate()
    {
        transform.position = target.transform.position + offset;
        transform.LookAt(target.transform); // camera�� target�� �ٶ�
    }
}
