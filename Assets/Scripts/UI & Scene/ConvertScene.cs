using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConvertScene : MonoBehaviour
{
    public Button mainMenu; // 'Main Menu' 이동 버튼
    public Button theOffice; // 'The Office' 맵 이동 버튼
    public Button tmp2; // 'temp' 맵 이동 버튼
    public Button tmp3; // 'temp' 맵 이동 버튼
    public Button tmp4; // 'temp' 맵 이동 버튼
    public Button tmp5; // 'temp' 맵 이동 버튼

    private LoadingSceneManager loadingManager;

    private void Start()
    {
        // 로딩 매니저 컴포넌트 찾기
        loadingManager = FindObjectOfType<LoadingSceneManager>();

        // 버튼에 클릭 이벤트 연결
        if (mainMenu != null) mainMenu.onClick.AddListener(() => OnButtonClicked(0));
        if (theOffice != null) theOffice.onClick.AddListener(() => OnButtonClicked(1));
        if (tmp2 != null) tmp2.onClick.AddListener(() => OnButtonClicked(2));
        if (tmp3 != null) tmp3.onClick.AddListener(() => OnButtonClicked(3));
        if (tmp4 != null) tmp4.onClick.AddListener(() => OnButtonClicked(4));
        if (tmp5 != null) tmp5.onClick.AddListener(() => OnButtonClicked(5));
    }

    void OnButtonClicked(int sceneIndex)
    {
        // 로딩 매니저를 통해 씬 로드
        if (loadingManager != null)
        {
            loadingManager.LoadScene(sceneIndex);
        }
    }
}
