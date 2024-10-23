using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MissionFailed : MonoBehaviour
{
    [Header("Main Camera")]
    [SerializeField] Camera mainCamera; // ���� ī�޶�

    [Header("Mission Failed UI")]
    [SerializeField] string missionCanvasPrefabPath = "Prefabs/UI/WhetherMissionCompleted";  // ĵ���� ������ ���
    [SerializeField] GameObject missionCanvas; // ���� ������ �̼� ĵ����
    [SerializeField] GameObject missionFailedPanel; // �̼� ���� �г�

    private Button quitButton; // Quit ��ư
    private Button restartButton; // Restart ��ư

    private PlayerStatus _player;
    private bool isMissionFailed = false;

    void Start()
    {
        // Main Camera �ڵ� ����
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("Main Camera�� ã�� �� �����ϴ�. ���� ī�޶� �������ּ���.");
                return;
            }
        }

        // PlayerStatus �ʱ�ȭ
        _player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerStatus>();

        if (_player == null)
        {
            Debug.LogError("Player�� ã�� �� ���ų� PlayerStatus ������Ʈ�� �����ϴ�.");
            return;
        }

        LoadUI();
    }

    void Update()
    {
        // �÷��̾ ����� ��� �̼� ���� â ǥ��
        if (_player != null && _player.IsAlive == false)
        {
            if (!isMissionFailed)
            {
                ShowMissionFailedPanel();
                isMissionFailed = true;
                StartCoroutine(PauseGameAfterDelay());
            }
        }
    }

    void LoadUI()
    {
        // ���ҽ� ��ο��� ĵ���� ������ �ε�
        GameObject missionCanvasPrefab = Resources.Load<GameObject>(missionCanvasPrefabPath);

        // �������� ����� �ε�Ǿ����� Ȯ��
        if (missionCanvasPrefab == null)
        {
            Debug.LogError($"�������� {missionCanvasPrefabPath} ��ο��� ã�� �� �����ϴ�.");
            return;
        }

        // �������� ĵ���� ���� �� �߰�
        missionCanvas = Instantiate(missionCanvasPrefab, transform);
        Debug.Log("Mission canvas instantiated.");

        // �ڽ� ������Ʈ���� �г��� ã�� ����
        Transform missionCanvasTransform = missionCanvas.transform.Find("Canvas");
        if (missionCanvasTransform == null)
        {
            Debug.LogError("Canvas ������Ʈ�� ã�� �� �����ϴ�. Mission UI�� ������ Ȯ�����ּ���.");
            return;
        }

        missionFailedPanel = missionCanvasTransform.Find("MissionFailed_Panel")?.gameObject;

        if (missionFailedPanel != null)
        {
            missionFailedPanel.SetActive(false);  // �ʱ⿡�� ��Ȱ��ȭ
            Debug.Log("MissionFailed_Panel loaded successfully.");

            // GetComponentInChildren�� ����� QuitButton �� RestartButton ã��
            quitButton = missionFailedPanel.transform.Find("QuitButton")?.GetComponent<Button>();
            restartButton = missionFailedPanel.transform.Find("RestartButton")?.GetComponent<Button>();

            // ��ư�� ����� ã�������� Ȯ��
            if (quitButton == null || restartButton == null)
            {
                Debug.LogError("QuitButton �Ǵ� RestartButton�� ã�� �� �����ϴ�.");
                return;
            }

            // ��ư �̸��� Ȯ�� �� ������ �̺�Ʈ ������ �߰�
            if (quitButton.name == "QuitButton")
            {
                quitButton.onClick.AddListener(QuitToMainMenu); // Quit ��ư Ŭ�� �̺�Ʈ ���
            }
            if (restartButton.name == "RestartButton")
            {
                restartButton.onClick.AddListener(RestartScene); // Restart ��ư Ŭ�� �̺�Ʈ ���
            }
        }
        else
        {
            Debug.LogError("MissionFailed_Panel�� ã�� �� �����ϴ�. ��θ� Ȯ�����ּ���.");
        }
    }

    void ShowMissionFailedPanel()
    {
        if (missionFailedPanel != null)
        {
            PauseGameAfterDelay();
            missionFailedPanel.SetActive(true); // �̼� ���� UI Ȱ��ȭ
            Debug.Log("MissionFailed_Panel activated.");
            Time.timeScale = 0f; // �ð�����
        }
    }

    // �ǰ� 0%�� �� �� �̼� ���� â �˾�
    IEnumerator PauseGameAfterDelay()
    {
        yield return new WaitForSeconds(3f); // 1�� ��� ��
    }

    // Quit ��ư Ŭ�� �� ���� �޴��� �̵�
    void QuitToMainMenu()
    {
        Debug.Log("Quit to Main Menu ��ư Ŭ����.");
        Time.timeScale = 1f; // �ð� �ٽ� �帣��
        SceneManager.LoadScene(0); // ���� �޴� ������ �̵�
    }

    // Restart ��ư Ŭ�� �� ���� �� �ٽ� ����
    void RestartScene()
    {
        Debug.Log("Restart Scene ��ư Ŭ����.");
        Time.timeScale = 1f; // �ð� �ٽ� �帣��
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // ���� �� �ٽ� ����
    }
}
