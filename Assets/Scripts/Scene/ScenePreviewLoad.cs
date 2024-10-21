using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePreviewLoad : MonoBehaviour
{
    public string sceneToLoad = "Office_Background"; // 로드할 다른 씬의 이름

    void Start()
    {
        // 다른 씬을 Additive 모드로 비동기 로드
        SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive).completed += OnSceneLoaded;
    }

    void OnSceneLoaded(AsyncOperation op)
    {
        Debug.Log(sceneToLoad + " 로드 완료");
        // 추가적으로 필요한 설정이 있다면 여기서 처리
    }
}
