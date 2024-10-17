using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConvertScene : MonoBehaviour
{
    public Button mainMenu; // 'Main Menu' �̵� ��ư
    public Button theOffice; // 'The Office' �� �̵� ��ư
    public Button tmp2; // 'temp' �� �̵� ��ư
    public Button tmp3; // 'temp' �� �̵� ��ư
    public Button tmp4; // 'temp' �� �̵� ��ư
    public Button tmp5; // 'temp' �� �̵� ��ư

    private LoadingSceneManager loadingManager;

    private void Start()
    {
        // �ε� �Ŵ��� ������Ʈ ã��
        loadingManager = FindObjectOfType<LoadingSceneManager>();

        // ��ư�� Ŭ�� �̺�Ʈ ����
        if (mainMenu != null) mainMenu.onClick.AddListener(() => OnButtonClicked(0));
        if (theOffice != null) theOffice.onClick.AddListener(() => OnButtonClicked(1));
        if (tmp2 != null) tmp2.onClick.AddListener(() => OnButtonClicked(2));
        if (tmp3 != null) tmp3.onClick.AddListener(() => OnButtonClicked(3));
        if (tmp4 != null) tmp4.onClick.AddListener(() => OnButtonClicked(4));
        if (tmp5 != null) tmp5.onClick.AddListener(() => OnButtonClicked(5));
    }

    void OnButtonClicked(int sceneIndex)
    {
        // �ε� �Ŵ����� ���� �� �ε�
        if (loadingManager != null)
        {
            loadingManager.LoadScene(sceneIndex);
        }
    }
}
