using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    public void OnButtonPressed()
    {
        // ��ư�� ���ȴٴ� ���¸� ����
        PlayerPrefs.SetInt("ButtonPressed", 1);
        PlayerPrefs.Save();

        // ���� ������ �̵�
        SceneManager.LoadScene("NextScene");
    }
}