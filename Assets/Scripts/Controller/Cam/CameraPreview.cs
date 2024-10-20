using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPreview : MonoBehaviour
{
    public RenderTexture previewTexture;

    void Start()
    {
        Camera camera = GetComponent<Camera>();
        camera.targetTexture = previewTexture;
    }
}
