using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // UI�� ����ϱ� ���� ���ӽ����̽�

public class MenuController : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] GameObject pauseMenuUI; // ESC ������ �� Ȱ��ȭ�� UI
    [SerializeField] Button uiPauseButton; // UI ��ư

    private bool isPaused = false; // ������ �Ͻ� ���� �������� ����

    void Start()
    {
        // UI ��ư�� OnClick �̺�Ʈ�� TogglePauseMenu ����
        if (uiPauseButton != null)
        {
            uiPauseButton.onClick.AddListener(TogglePauseMenu);
        }
    }

    void Update()
    {
        // ESC Ű �Է� ����
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    void TogglePauseMenu()
    {
        isPaused = !isPaused; // �Ͻ� ���� ���� ���
        pauseMenuUI.SetActive(isPaused); // UI Ȱ��ȭ/��Ȱ��ȭ

        if (isPaused)
        {
            Time.timeScale = 0; // ���� �Ͻ� ����
        }
        else
        {
            Time.timeScale = 1; // ���� �簳
        }
    }
}
