using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour, IManager
{
    public System.Action UpdateDelegate = null;
    public System.Action LateDelegate = null;

    // 옵션 UI 관련
    public static GameState gameState;
    public static GameObject gameOption; // 옵션 화면 UI 오브젝트
    public static GameObject gameLabel; // 게임 상태 UI 오브젝트
    public static Text gameText; // 게임 상태 UI 텍스트

    public void Updater()
    {
        if (UpdateDelegate != null) { UpdateDelegate(); }
    }
    public void LateUpdater()
    {
        if (LateDelegate != null) { LateDelegate(); }
    }

    // 옵션
    public void OpenOptionWindow() // 옵션 화면 켜기
    {
        gameOption.SetActive(true);
        Time.timeScale = 0f; // 게임속도 0배속
        gameState = GameState.Pause; // 일시정지 상태
    }
    public void CloseOptionWindow()
    {
        gameOption.SetActive(false);
        Time.timeScale = 1f; // 게임속도 1배속
        gameState = GameState.Run; // 게임 중 상태
    }
    public void RestartGame()
    {
        Time.timeScale = 1f; // 게임속도 1배속
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ExitGame() // 게임 종료
    {
        Application.Quit();
    }

    public void InitGameState() // 초기 게임상태 설정
    {
        if (gameLabel != null)
        {
            gameText = gameLabel.GetComponent<Text>(); // 게임상태UI옵젝에서 Text 컴포넌트 가져옴
            gameText.text = "Ready";
            gameText.color = new Color32(0, 0, 0, 255); // 텍스트 검은색

            gameState = GameState.Ready; // 게임 상태
        }
        else
        {
            Debug.Log("The gameLabel is null!");
        }
        
    }
    public IEnumerator ReadyToStart() // 게임 시작 전 준비 상태 유지
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
