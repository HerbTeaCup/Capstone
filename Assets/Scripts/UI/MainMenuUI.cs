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
        Debug.Log("���� ����");
    }

    public void OnClickCheckKeys()
    {
        Debug.Log("Ű Ȯ��");
    }

    public void OnClickSettings()
    {
        Debug.Log("���� ����");
    }

    public void OnClickExit()
    {
        Debug.Log("���� ����");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // �÷��� ���� �ߴ�
#else
        Application.Quit(); // �����Ϳ����� �۵� X
#endif
    }
}
