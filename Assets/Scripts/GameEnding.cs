using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnding : MonoBehaviour
{
    public float fadeDuration = 1f;
    public float displayImageDuration = 1f;
    public GameObject player;
    public CanvasGroup exitBackgroundImageCanvasGroup; // 탈출했을 경우
    public CanvasGroup caughtBackgroundImageCanvasGroup; // 잡혔을 경우

    bool m_IsPlayerAtExit; // 출구에 도착했는지
    bool m_IsPlayerCaught; // 적에게 잡혔는지
    float m_Timer;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            m_IsPlayerAtExit = true;
        }
    }
    public void CaughtPlayer()
    {
        m_IsPlayerCaught = true;
    }

    void Update()
    {
        if (m_IsPlayerAtExit) // 탈출?
        {
            EndLevel(exitBackgroundImageCanvasGroup, false);
        }
        else if (m_IsPlayerCaught) // 잡힘?
        {
            EndLevel(caughtBackgroundImageCanvasGroup, true);
        }
    }

    void EndLevel(CanvasGroup imageCanvasGroup, bool doRestart) // doRestart가 true이면 레벨 재시작 -> 씬 다시 로드
    {
        m_Timer += Time.deltaTime;
        imageCanvasGroup.alpha = m_Timer / fadeDuration;

        if (m_Timer > fadeDuration + displayImageDuration)
        {
            if (doRestart)
            {
                SceneManager.LoadScene(0); // 씬 0 다시 로드 // 정적메서드(클래스의 인스턴스 없이도 호출 가능) LoadScene 호출
            }
            else
            {
                Application.Quit();
            }
        }
    }
}
