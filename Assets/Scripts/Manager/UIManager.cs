using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour, IManager
{
    public GameObject[] uiElementToHide; // 숨길 UI 배열
    public GameObject[] otherUIElements; // 숨기지 않을 UI 배열

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

    // UI 숨기기 함수
    public void HideUI()
    {
        // 이전 씬에서 버튼이 눌렸는지 확인
        if (PlayerPrefs.GetInt("ButtonPressed", 0) == 1)
        {
            // 특정 UI 숨기기
            if (uiElementToHide != null)
            {
                foreach (GameObject uiElement in uiElementToHide)
                {
                    uiElement.SetActive(false);
                }
            }
        }
    }

    // UI 상태 초기화 함수
    public void ResetUI()
    {
        // 다른 UI 요소들은 보이게 설정
        foreach (GameObject uiElement in otherUIElements)
        {
            if (uiElement != null)
            {
                uiElement.SetActive(true);
                // 자식 UI 요소도 개별적으로 활성화
                foreach (Transform child in uiElement.transform)
                {
                    child.gameObject.SetActive(true);
                }
            }
        }

        // 숨긴 UI 다시 보이게 설정
        if (uiElementToHide != null)
        {
            foreach (GameObject uiElement in uiElementToHide)
            {
                uiElement.SetActive(true);
            }
        }

        // 자식 UI의 alpha 값을 1로 설정
        SetChildUIAlpha(this.gameObject, 1f);
    }
    // 자식 UI의 alpha 값을 설정하는 함수
    private void SetChildUIAlpha(GameObject parent, float alpha)
    {
        // 자식 요소가 CanvasGroup이 있는지 확인
        foreach (Transform child in parent.transform)
        {
            CanvasGroup canvasGroup = child.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = alpha; // alpha 값 설정
            }

            // 재귀적으로 자식 요소의 alpha 값도 설정
            SetChildUIAlpha(child.gameObject, alpha);
        }
    }

    public void ShowUI()
    {
        // UIManager를 활성화하고 모든 자식 UI를 활성화
        this.gameObject.SetActive(true);

        // 자식 UI 요소들을 개별적으로 활성화
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
