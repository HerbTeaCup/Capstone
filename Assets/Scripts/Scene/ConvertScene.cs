using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConvertScene : MonoBehaviour
{
    public Button mainMenu; // 'Main Menu' 이동 버튼
    public Button theOffice; // 'The Office' 맵 이동 버튼
    public Button theParkingLot; // 'The Parking Lot' 맵 이동 버튼
    public Button theWarehouse; // 'The Warehouse' 맵 이동 버튼
    // public Button tmp4; // 'temp' 맵 이동 버튼
    // public Button tmp5; // 'temp' 맵 이동 버튼

    private LoadingSceneManager loadingManager;

    private void Start()
    {
        // 로딩 매니저 컴포넌트 찾기
        loadingManager = GameManager.LoadingScene;
        if (loadingManager == null)
        {
            Debug.LogError("LoadingSceneManager가 존재하지 않습니다.");
        }
        else
        {
            Debug.Log("LoadingSceneManager가 정상적으로 연결되었습니다.");
        }

        // 버튼에 클릭 이벤트 연결
        if (mainMenu != null) mainMenu.onClick.AddListener(() => OnButtonClicked(0));
        if (theOffice != null) theOffice.onClick.AddListener(() => OnButtonClicked(1));
        if (theParkingLot != null) theParkingLot.onClick.AddListener(() => OnButtonClicked(2));
        if (theWarehouse != null) theWarehouse.onClick.AddListener(() => OnButtonClicked(3));
        // if (tmp4 != null) tmp4.onClick.AddListener(() => OnButtonClicked(4));
        // if (tmp5 != null) tmp5.onClick.AddListener(() => OnButtonClicked(5));
    }

    void OnButtonClicked(int sceneIndex)
    {
        Time.timeScale = 1; // 씬 전환 전 시간 초기화
        // 로딩 매니저를 통해 씬 로드
        if (loadingManager != null)
        {
            Debug.Log($"LoadScene({sceneIndex}) 호출됨");
            loadingManager.LoadScene(sceneIndex);
            // loadingManager.LoadSceneWithInit(sceneIndex);
        }
        else
        {
            Debug.LogError("LoadingSceneManager가 연결되지 않았습니다.");
        }
    }
}
