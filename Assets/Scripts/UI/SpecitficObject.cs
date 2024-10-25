using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpecitficObject : InteractableObjExtand
{
    [SerializeField] private string gameClearPanelPrefabPath = "Prefabs/UI/WhetherMissionCompleted";
    [SerializeField] private GameObject missionClearPanel; // �̼� Ŭ���� UI
    [SerializeField] private string targetSceneName = "MainMenuScene"; // ��ȯ�� �� �̸�
    [SerializeField] private float triggerDistance = 5f; // Ʈ���� ����
    // [SerializeField] private int sortingOrder = 100; // �ٸ� UI�� ���� ���� ����

    Transform player;
    private Button quitButton; // Quit ��ư
    private GameObject missionCompletedPanel;
    private Canvas clearPanelCanvas;

    private bool isUIShown = false;

    void Start()
    {
        gameObject.SetActive(false);

        // �÷��̾ null���� üũ�ϰ� �ڵ����� �Ҵ�
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

            if (player == null)
            {
                Debug.LogError("Player�� �������� �ʾҽ��ϴ�. Inspector���� �����ϰų� �±׸� Ȯ���ϼ���.");
            }

        }
        // �̼� Ŭ���� UI �̸� �ε�
        LoadUI();

    }

    private void Update()
    {
        if (player == null) return; // �÷��̾ ���� ��� Update ����

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Ư�� �Ÿ� �̳��� �÷��̾ �������� �� UI�� ǥ��
        if (distanceToPlayer <= triggerDistance && !isUIShown)
        {
            ShowGameClearPanel();
        }
    }

    public override void Interaction()
    {
        if (!interactable) return;

        // �̼� Ŭ���� UI ǥ��
        ShowGameClearPanel();
    }

    private void LoadUI()
    {
        if (missionClearPanel == null)
        {
            GameObject gameClearPrefab = Resources.Load<GameObject>(gameClearPanelPrefabPath);
            if (gameClearPrefab != null)
            {
                missionClearPanel = Instantiate(gameClearPrefab, transform);
                Debug.Log("gameClearPanel ������ �ε� ����");

                // ��Ȯ�� ��η� MissionCompleted_Panel�� ã��
                Transform canvasTransform = missionClearPanel.transform.Find("Canvas");
                if (canvasTransform != null)
                {
                    missionCompletedPanel = canvasTransform.Find("MissionCompleted_Panel")?.gameObject;
                    if (missionCompletedPanel != null)
                    {
                        Debug.Log("MissionCompleted_Panel�� ���������� ã�ҽ��ϴ�.");
                        missionCompletedPanel.SetActive(false); // �ʱ⿡�� ��Ȱ��ȭ

                        // RectTransform���� UI ��ġ ����
                        RectTransform rectTransform = missionCompletedPanel.GetComponent<RectTransform>();
                        if (rectTransform != null)
                        {
                            // Anchors�� �߾ӿ� ����
                            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                            rectTransform.pivot = new Vector2(0.5f, 0.5f);

                            // �ʱ� ��ġ ���� (ȭ���� �߾�)
                            rectTransform.anchoredPosition = Vector2.zero;
                        }
                    }
                    else
                    {
                        Debug.LogError("MissionCompleted_Panel�� ã�� �� �����ϴ�.");
                    }
                }
                else
                {
                    Debug.LogError("Canvas�� ã�� �� �����ϴ�.");
                }
            }
            else
            {
                Debug.LogError($"�������� {gameClearPanelPrefabPath} ��ο��� �ε��� �� �����ϴ�.");
            }
        }
    }

    private void ShowGameClearPanel()
    {
        // ���� Ŭ���� �г��� Ȱ��ȭ
        if (missionCompletedPanel != null)
        {
            missionCompletedPanel.SetActive(true);
            isUIShown = true; // UI�� �� ���� ��Ÿ������ ����
            Debug.Log("MissionCompleted_Panel�� Ȱ��ȭ�Ǿ����ϴ�.");

            // Quit ��ư ������Ʈ ã��
            quitButton = missionCompletedPanel.transform.Find("QuitButton")?.GetComponent<Button>();
            if (quitButton != null)
            {
                // Quit ��ư Ŭ�� �� �� ��ȯ �̺�Ʈ �߰�
                quitButton.onClick.AddListener(OnQuitButtonClick);
            }
            else
            {
                Debug.LogError("QuitButton ��ư�� ã�� �� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogError("MissionCompleted_Panel�� ã�� �� �����ϴ�.");
        }

        missionClearPanel.SetActive(true); // ��ü �г� Ȱ��ȭ
    }

    // Quit ��ư Ŭ�� �� �� ��ȯ ó��
    private void OnQuitButtonClick()
    {
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName); // MainMenuScene���� ��ȯ
            Debug.Log($"{targetSceneName} ������ ��ȯ ��...");
        }
        else
        {
            Debug.LogError("��ȯ�� �� �̸��� �������� �ʾҽ��ϴ�.");
        }
    }

}
