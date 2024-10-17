using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
    public static bool IsInitialized = false;
  
    private void Start()
    {
        InitializeScene();
    }

    void InitializeScene()
    {
        // 씬의 모든 초기화 로직
        // 초기화 완료 후
        IsInitialized = true;
    }
}
