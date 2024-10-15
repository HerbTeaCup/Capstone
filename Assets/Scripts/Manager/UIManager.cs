using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour, IManager
{
    static UIManager _instance = null;

    public static UIManager Instance { get { Init(); return _instance; } }

    public GameObject[] uiElementToHide; // ���� UI �迭
    public GameObject[] otherUIElements; // ������ ���� UI �迭

    // �ʱ�ȭ �޼ҵ�
    static void Init()
    {
        if (_instance == null)
        {
            GameObject temp = GameObject.Find("@UIManager");

            if (temp == null)
            {
                temp = new GameObject("@UIManager");
            }

            temp.TryGetComponent<UIManager>(out _instance);
            if (_instance == null) { _instance = temp.AddComponent<UIManager>(); }
        }
    }

    void Awake()
    {
        ResetUI();
        HideUI();
        ShowUI();
    }

    // UI ����� �Լ�
    public void HideUI()
    {
        // ���� ������ ��ư�� ���ȴ��� Ȯ��
        if (PlayerPrefs.GetInt("ButtonPressed", 0) == 1)
        {
            // Ư�� UI �����
            if (uiElementToHide != null)
            {
                foreach (GameObject uiElement in uiElementToHide)
                {
                    uiElement.SetActive(false);
                }
            }
        }
    }

    // UI ���� �ʱ�ȭ �Լ�
    public void ResetUI()
    {
        // �ٸ� UI ��ҵ��� ���̰� ����
        foreach (GameObject uiElement in otherUIElements)
        {
            if (uiElement != null)
            {
                uiElement.SetActive(true);
                // �ڽ� UI ��ҵ� ���������� Ȱ��ȭ
                foreach (Transform child in uiElement.transform)
                {
                    child.gameObject.SetActive(true);
                }
            }
        }

        // ���� UI �ٽ� ���̰� ����
        if (uiElementToHide != null)
        {
            foreach (GameObject uiElement in uiElementToHide)
            {
                uiElement.SetActive(true);
            }
        }

        // �ڽ� UI�� alpha ���� 1�� ����
        SetChildUIAlpha(this.gameObject, 1f);
    }
    // �ڽ� UI�� alpha ���� �����ϴ� �Լ�
    private void SetChildUIAlpha(GameObject parent, float alpha)
    {
        // �ڽ� ��Ұ� CanvasGroup�� �ִ��� Ȯ��
        foreach (Transform child in parent.transform)
        {
            CanvasGroup canvasGroup = child.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = alpha; // alpha �� ����
            }

            // ��������� �ڽ� ����� alpha ���� ����
            SetChildUIAlpha(child.gameObject, alpha);
        }
    }

    public void ShowUI()
    {
        // UIManager�� Ȱ��ȭ�ϰ� ��� �ڽ� UI�� Ȱ��ȭ
        this.gameObject.SetActive(true);

        // �ڽ� UI ��ҵ��� ���������� Ȱ��ȭ
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    public void Clear()
    {
        ResetUI();
    }
}
