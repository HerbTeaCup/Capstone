using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    public void OnButtonPressed()
    {
        // 버튼이 눌렸다는 상태를 저장
        PlayerPrefs.SetInt("ButtonPressed", 1);
        PlayerPrefs.Save();

        // 다음 씬으로 이동
        SceneManager.LoadScene("NextScene");
    }
}