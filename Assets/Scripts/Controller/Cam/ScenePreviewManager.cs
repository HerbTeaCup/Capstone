using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePreviewManager : MonoBehaviour
{
    public string sceneToLoad;  // �ε��� �� �̸�
    public Camera previewCamera; // �̸������ ī�޶�
    public RenderTexture previewTexture; // �������� �ؽ�ó

    void Start()
    {
        // ���� Additive ���� �񵿱� �ε�
        SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive).completed += OnSceneLoaded;
    }

    // �� �ε� �Ϸ� �� ī�޶� ����
    void OnSceneLoaded(AsyncOperation op)
    {
        // �ε�� ������ ī�޶� ã�Ƽ� ������ �ؽ�ó ����
        GameObject[] rootObjects = SceneManager.GetSceneByName(sceneToLoad).GetRootGameObjects();
        foreach (GameObject obj in rootObjects)
        {
            Camera sceneCamera = obj.GetComponentInChildren<Camera>();
            if (sceneCamera != null)
            {
                sceneCamera.targetTexture = previewTexture;
                break;
            }
        }
    }
}
