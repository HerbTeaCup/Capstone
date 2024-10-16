using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour, IManager
{
    public GameObject[] uiElementToHide; // ���� UI �迭
    public GameObject[] otherUIElements; // ������ ���� UI �迭

    void Awake()
    {
        
    }
    private void Start()
    {
        GameManager.UI = this;

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
