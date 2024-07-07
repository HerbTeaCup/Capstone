using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToCamManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Cam.AddCamera(this.gameObject.name, this.transform.GetComponent<Cinemachine.CinemachineVirtualCameraBase>());
    }
}
