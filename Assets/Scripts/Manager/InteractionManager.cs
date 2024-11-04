using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public Transform player;
    public GameObject interactionUI;
    public GameObject objectToActivate;
    public Outline outline;

    public float interactionDistance = 5f;
    private bool inRange = false;
    public KeyCode interactionKey = KeyCode.F;
    public Vector3 offset = new Vector3(0, 0f, 0); // 머리 위로 올리기 위해 Y 값을 조정
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        interactionUI.SetActive(false);
        outline.enabled = false;
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= interactionDistance)
        {
            if (!inRange)
            {
                inRange = true;
                ShowOutline(true);
                interactionUI.SetActive(true);
            }

            // 월드 공간에서 UI의 위치 설정
            interactionUI.transform.position = transform.position + offset;
            interactionUI.transform.LookAt(mainCamera.transform); // UI가 카메라를 바라보도록 설정

            // UI의 좌우반전을 위해 로컬 스케일 조정
            Vector3 scale = interactionUI.transform.localScale;
            scale.x = -Mathf.Abs(scale.x); // X축을 반전
            interactionUI.transform.localScale = scale;

            if (Input.GetKeyDown(interactionKey))
            {
                ActivateObject();
            }
        }
        else
        {
            if (inRange)
            {
                inRange = false;
                ShowOutline(false);
                interactionUI.SetActive(false);
            }
        }
    }

    void ShowOutline(bool show)
    {
        if (outline != null)
        {
            outline.enabled = show;
        }
    }

    void ActivateObject()
    {
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(!objectToActivate.activeSelf);
        }
    }
}
