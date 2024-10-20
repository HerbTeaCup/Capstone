using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePreviewLoad : MonoBehaviour
{
    public string sceneToLoad = "Office_Background"; // �ε��� �ٸ� ���� �̸�

    void Start()
    {
        // �ٸ� ���� Additive ���� �񵿱� �ε�
        SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive).completed += OnSceneLoaded;
    }

    void OnSceneLoaded(AsyncOperation op)
    {
        Debug.Log(sceneToLoad + " �ε� �Ϸ�");
        // �߰������� �ʿ��� ������ �ִٸ� ���⼭ ó��
    }
}
