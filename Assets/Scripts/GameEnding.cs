using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnding : MonoBehaviour
{
    public float fadeDuration = 1f;
    public float displayImageDuration = 1f;
    public GameObject player;
    public CanvasGroup exitBackgroundImageCanvasGroup; // Ż������ ���
    public CanvasGroup caughtBackgroundImageCanvasGroup; // ������ ���

    bool m_IsPlayerAtExit; // �ⱸ�� �����ߴ���
    bool m_IsPlayerCaught; // ������ ��������
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
        if (m_IsPlayerAtExit) // Ż��?
        {
            EndLevel(exitBackgroundImageCanvasGroup, false);
        }
        else if (m_IsPlayerCaught) // ����?
        {
            EndLevel(caughtBackgroundImageCanvasGroup, true);
        }
    }

    void EndLevel(CanvasGroup imageCanvasGroup, bool doRestart) // doRestart�� true�̸� ���� ����� -> �� �ٽ� �ε�
    {
        m_Timer += Time.deltaTime;
        imageCanvasGroup.alpha = m_Timer / fadeDuration;

        if (m_Timer > fadeDuration + displayImageDuration)
        {
            if (doRestart)
            {
                SceneManager.LoadScene(0); // �� 0 �ٽ� �ε� // �����޼���(Ŭ������ �ν��Ͻ� ���̵� ȣ�� ����) LoadScene ȣ��
            }
            else
            {
                Application.Quit();
            }
        }
    }
}
