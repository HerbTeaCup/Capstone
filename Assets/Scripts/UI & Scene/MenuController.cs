using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI를 사용하기 위한 네임스페이스

public class MenuController : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] GameObject pauseMenuUI; // ESC 눌렀을 때 활성화할 UI
    [SerializeField] Button uiPauseButton; // UI 버튼

    private bool isPaused = false; // 게임이 일시 정지 상태인지 여부

    void Start()
    {
        // UI 버튼에 OnClick 이벤트로 TogglePauseMenu 연결
        if (uiPauseButton != null)
        {
            uiPauseButton.onClick.AddListener(TogglePauseMenu);
        }
    }

    void Update()
    {
        // ESC 키 입력 감지
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    void TogglePauseMenu()
    {
        isPaused = !isPaused; // 일시 정지 상태 토글
        pauseMenuUI.SetActive(isPaused); // UI 활성화/비활성화

        if (isPaused)
        {
            Time.timeScale = 0; // 게임 일시 정지
        }
        else
        {
            Time.timeScale = 1; // 게임 재개
        }
    }
}
