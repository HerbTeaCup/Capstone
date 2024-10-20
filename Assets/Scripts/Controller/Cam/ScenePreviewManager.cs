using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePreviewManager : MonoBehaviour
{
    public string sceneToLoad;  // 로드할 씬 이름
    public Camera previewCamera; // 미리보기용 카메라
    public RenderTexture previewTexture; // 렌더링할 텍스처

    void Start()
    {
        // 씬을 Additive 모드로 비동기 로드
        SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive).completed += OnSceneLoaded;
    }

    // 씬 로드 완료 후 카메라 설정
    void OnSceneLoaded(AsyncOperation op)
    {
        // 로드된 씬에서 카메라를 찾아서 렌더링 텍스처 설정
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
