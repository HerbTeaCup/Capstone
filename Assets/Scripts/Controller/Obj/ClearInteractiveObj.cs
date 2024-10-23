using UnityEngine;
using UnityEngine.SceneManagement; // �� ��ȯ�� ���� �߰�
using UnityEngine.UI; // UI ��ư�� �ٷ�� ���� �߰�

public class ClearInteractiveObj : InteractableObjExtand
{
    [SerializeField] private string gameClearPanelPrefabPath = "Prefabs/UI/WhetherMissionCompleted";
    [SerializeField] private string targetSceneName = "MainMenuScene"; // ��ȯ�� �� �̸�
    [SerializeField] private Transform targetObject;
    [SerializeField] private float clearDistance = 5f;

    private GameObject gameClearPanel;
    private GameObject missionCompletedPanel;
    private GameObject missionFailedPanel;
    private Button quitButton; // Quit ��ư
    private Button restartButton; // Restart ��ư

    private void Start()
    {
        gameObject.SetActive(false); // �ʱ� ��Ȱ��ȭ
    }

    public void SetInteractable(bool value)
    {
        interactable = value;
    }

    public override void Interaction()
    {
        if (!interactable) return;

        // ��� ������Ʈ���� �Ÿ� üũ
        float distanceToTarget = Vector3.Distance(transform.position, targetObject.position);
        if (distanceToTarget <= clearDistance)
        {
            ShowGameClearPanel(); // �г� ǥ��
        }
        else
        {
            Debug.Log("���� Ŭ���� ��ġ�� ���� ����.");
        }
    }

    // �÷��̾ �׾��� �� ȣ��
    public void PlayerDied()
    {
        ShowMissionFailedPanel();
    }

    private void ShowGameClearPanel()
    {
        if (gameClearPanel == null)
        {
            GameObject gameClearPrefab = Resources.Load<GameObject>(gameClearPanelPrefabPath);
            if (gameClearPrefab != null)
            {
                // ������ ����
                gameClearPanel = Instantiate(gameClearPrefab);
                Debug.Log("���� Ŭ���� �г��� �����Ǿ����ϴ�.");
            }
            else
            {
                Debug.LogError($"��ο��� ���� Ŭ���� �г� �������� ã�� �� �����ϴ�: {gameClearPanelPrefabPath}");
                return;
            }
        }

        // Canvas ���� MissionCompleted_Panel ã��
        missionCompletedPanel = gameClearPanel.transform.Find("Canvas/MissionCompleted_Panel")?.gameObject;
        if (missionCompletedPanel != null)
        {
            missionCompletedPanel.SetActive(true);
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

        gameClearPanel.SetActive(true);
    }

    private void ShowMissionFailedPanel()
    {
        if (gameClearPanel == null)
        {
            GameObject gameClearPrefab = Resources.Load<GameObject>(gameClearPanelPrefabPath);
            if (gameClearPrefab != null)
            {
                // ������ ����
                gameClearPanel = Instantiate(gameClearPrefab);
                Debug.Log("MissionFailed_Panel�� �����Ǿ����ϴ�.");
            }
            else
            {
                Debug.LogError($"��ο��� ���� Ŭ���� �г� �������� ã�� �� �����ϴ�: {gameClearPanelPrefabPath}");
                return;
            }
        }

        // Canvas ���� MissionFailed_Panel ã��
        missionFailedPanel = gameClearPanel.transform.Find("Canvas/MissionFailed_Panel")?.gameObject;
        if (missionFailedPanel != null)
        {
            missionFailedPanel.SetActive(true);
            Debug.Log("MissionFailed_Panel�� Ȱ��ȭ�Ǿ����ϴ�.");

            // Quit ��ư ������Ʈ ã��
            quitButton = missionFailedPanel.transform.Find("QuitButton")?.GetComponent<Button>();
            if (quitButton != null)
            {
                quitButton.onClick.AddListener(OnQuitButtonClick); // Quit ��ư Ŭ�� �� MainMenu�� �̵�
            }
            else
            {
                Debug.LogError("QuitButton ��ư�� ã�� �� �����ϴ�.");
            }

            // Restart ��ư ������Ʈ ã��
            restartButton = missionFailedPanel.transform.Find("RestartButton")?.GetComponent<Button>();
            if (restartButton != null)
            {
                restartButton.onClick.AddListener(OnRestartButtonClick); // Restart ��ư Ŭ�� �� ���� �� �����
            }
            else
            {
                Debug.LogError("RestartButton ��ư�� ã�� �� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogError("MissionFailed_Panel�� ã�� �� �����ϴ�.");
        }

        gameClearPanel.SetActive(true);
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

    // Restart ��ư Ŭ�� �� ���� �� ����� ó��
    private void OnRestartButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // ���� ���� �����
        Debug.Log("���� ���� ������մϴ�.");
    }
}
