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
        // ���� ��� �ʱ�ȭ ����
        // �ʱ�ȭ �Ϸ� ��
        IsInitialized = true;
    }
}
