using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickStartGame()
    {
        Debug.Log("게임 시작");
    }

    public void OnClickCheckKeys()
    {
        Debug.Log("키 확인");
    }

    public void OnClickSettings()
    {
        Debug.Log("게임 세팅");
    }

    public void OnClickExit()
    {
        Debug.Log("게임 종료");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 플레이 상태 중단
#else
        Application.Quit(); // 에디터에서는 작동 X
#endif
    }
}
