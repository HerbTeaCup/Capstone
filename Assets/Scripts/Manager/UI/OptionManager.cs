using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour, IManager
{
    public System.Action UpdateDelegate = null;
    public System.Action LateDelegate = null;

    // �ɼ� UI ����
    public static GameState gameState;
    public static GameObject gameOption; // �ɼ� ȭ�� UI ������Ʈ
    public static GameObject gameLabel; // ���� ���� UI ������Ʈ
    public static Text gameText; // ���� ���� UI �ؽ�Ʈ

    public void Updater()
    {
        if (UpdateDelegate != null) { UpdateDelegate(); }
    }
    public void LateUpdater()
    {
        if (LateDelegate != null) { LateDelegate(); }
    }

    // �ɼ�
    public void OpenOptionWindow() // �ɼ� ȭ�� �ѱ�
    {
        gameOption.SetActive(true);
        Time.timeScale = 0f; // ���Ӽӵ� 0���
        gameState = GameState.Pause; // �Ͻ����� ����
    }
    public void CloseOptionWindow()
    {
        gameOption.SetActive(false);
        Time.timeScale = 1f; // ���Ӽӵ� 1���
        gameState = GameState.Run; // ���� �� ����
    }
    public void RestartGame()
    {
        Time.timeScale = 1f; // ���Ӽӵ� 1���
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ExitGame() // ���� ����
    {
        Application.Quit();
    }

    public void InitGameState() // �ʱ� ���ӻ��� ����
    {
        if (gameLabel != null)
        {
            gameText = gameLabel.GetComponent<Text>(); // ���ӻ���UI�������� Text ������Ʈ ������
            gameText.text = "Ready";
            gameText.color = new Color32(0, 0, 0, 255); // �ؽ�Ʈ ������

            gameState = GameState.Ready; // ���� ����
        }
        else
        {
            Debug.Log("The gameLabel is null!");
        }
        
    }
    public IEnumerator ReadyToStart() // ���� ���� �� �غ� ���� ����
    {
        yield return new WaitForSeconds(2f);
        if (gameText != null)
        {
            gameText.text = "Start";
        }
        yield return new WaitForSeconds(0.5f);

        if (gameLabel != null)
        {
            gameLabel.SetActive(false);
            gameState = GameState.Run;
        }
    }

    public void Clear()
    {
        UpdateDelegate = null;
        LateDelegate = null;
    }
}
