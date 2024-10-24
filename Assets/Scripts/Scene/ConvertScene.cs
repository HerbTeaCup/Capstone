using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConvertScene : MonoBehaviour
{
    public Button mainMenu; // 'Main Menu' �̵� ��ư
    public Button theOffice; // 'The Office' �� �̵� ��ư
    public Button theParkingLot; // 'The Parking Lot' �� �̵� ��ư
    public Button theWarehouse; // 'The Warehouse' �� �̵� ��ư
    // public Button tmp4; // 'temp' �� �̵� ��ư
    // public Button tmp5; // 'temp' �� �̵� ��ư

    private LoadingSceneManager loadingManager;

    private void Start()
    {
        // �ε� �Ŵ��� ������Ʈ ã��
        loadingManager = GameManager.LoadingScene;
        if (loadingManager == null)
        {
            Debug.LogError("LoadingSceneManager�� �������� �ʽ��ϴ�.");
        }
        else
        {
            Debug.Log("LoadingSceneManager�� ���������� ����Ǿ����ϴ�.");
        }

        // ��ư�� Ŭ�� �̺�Ʈ ����
        if (mainMenu != null) mainMenu.onClick.AddListener(() => OnButtonClicked(0));
        if (theOffice != null) theOffice.onClick.AddListener(() => OnButtonClicked(1));
        if (theParkingLot != null) theParkingLot.onClick.AddListener(() => OnButtonClicked(2));
        if (theWarehouse != null) theWarehouse.onClick.AddListener(() => OnButtonClicked(3));
        // if (tmp4 != null) tmp4.onClick.AddListener(() => OnButtonClicked(4));
        // if (tmp5 != null) tmp5.onClick.AddListener(() => OnButtonClicked(5));
    }

    void OnButtonClicked(int sceneIndex)
    {
        Time.timeScale = 1; // �� ��ȯ �� �ð� �ʱ�ȭ
        // �ε� �Ŵ����� ���� �� �ε�
        if (loadingManager != null)
        {
            Debug.Log($"LoadScene({sceneIndex}) ȣ���");
            loadingManager.LoadScene(sceneIndex);
            // loadingManager.LoadSceneWithInit(sceneIndex);
        }
        else
        {
            Debug.LogError("LoadingSceneManager�� ������� �ʾҽ��ϴ�.");
        }
    }
}
