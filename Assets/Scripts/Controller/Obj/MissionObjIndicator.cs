using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionObjIndicator : MonoBehaviour
{
    [Header("Main Camera")]
    [SerializeField] Camera mainCamera; // ���� ī�޶�

    [Header("Target(Object) UI")]
    [SerializeField] string targetCanvasPrefabPath = "Prefabs/UI/TargetIndicator/Target Enemy Canvas";  // ĵ���� ������ ���
    [SerializeField] string missionCanvasPrefabPath = "Prefabs/UI/TargetIndicator/Mission Canvas";  // ĵ���� ������ ���

    [Header("Target Distance Settings")]
    [SerializeField] float activationDistance = 20f; // ��ǥ Ȱ��ȭ �Ÿ�
    [SerializeField] float deactivationDistance = 20f; // ��ǥ ��Ȱ��ȭ �Ÿ�

    [Header("Object UI Settings")]
    [SerializeField] Transform objectUIPanel; // ��� ������Ʈ�� UI�� ǥ���� �г�

    [Header("Mission UI Settings")]
    [SerializeField] GameObject missionFailedPanel; // �̼� ���� UI
    [SerializeField] GameObject missionCompletedPanel; // �̼� �Ϸ� UI

    private Transform player; // �÷��̾��� Transform

    public bool isMissionCompleted = false;
    public bool hasConvertText = false;

    // ���� �ε� UI
    private Image targetImage;  // ��ǥ ��ġ ���ü� �̹���
    private Text targetDistanceText;  // ��ǥ ��ġ �ؽ�Ʈ
    private Image missionCompletedImage; // �̼� �ϼ� �̹���
    private Text missionCompletedText; // �̼� �ϼ� �ؽ�Ʈ

    private RectTransform imageRect; // UI �̹����� RectTransform
    private RectTransform textRect; // UI �ؽ�Ʈ�� RectTransform
    private RectTransform missionImageRect; // UI �̹����� RectTransform
    private RectTransform missionTextRect; // UI �ؽ�Ʈ�� RectTransform

    private GameObject targetCanvas; // ��ǥ ��ġ Canvas
    private GameObject missionCanvas; // �̼� Canvas

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

        TargetLoadUI();
        MissionLoadUI();

        GameManager.Object.UpdateDelegate += UpdateUI;
    }

    #region UI ������ �ε� �� �߰� �޼���
    // UI ������ �ε� �� �߰�
    void TargetLoadUI()
    {
        // ���ҽ� ��ο��� ĵ���� ������ �ε�
        GameObject targetCanvasPrefab = ResourceManager.Load<GameObject>(targetCanvasPrefabPath);
        CheckPrefabLoad(targetCanvasPrefab); // ������ �ε� ���� üũ

        // �������� ĵ���� ���� �� ObjectIndicator�� �߰�
        targetCanvas = Instantiate(targetCanvasPrefab, transform);

        // �ڽ� ������Ʈ���� �̹����� �ؽ�Ʈ�� ã�� ����
        targetImage = targetCanvas.transform.Find("Position Image")?.GetComponent<Image>();
        targetDistanceText = targetCanvas.transform.Find("Distance Text")?.GetComponent<Text>();
        #region Image �� Text ������Ʈ ���� Ȯ��
        if (CheckImage(targetImage) == false) Debug.Log(":Position Image");
        if (CheckText(targetDistanceText) == false) Debug.Log(":Distance Text");
        #endregion

        if (targetImage != null)
        {
            imageRect = targetImage.GetComponent<RectTransform>();
            targetImage.enabled = false;  // �ʱ⿡�� ��Ȱ��ȭ
        }

        if (targetDistanceText != null)
        {
            textRect = targetDistanceText.GetComponent<RectTransform>();
            targetDistanceText.enabled = false;  // �ʱ⿡�� ��Ȱ��ȭ
        }
    }

    void MissionLoadUI()
    {
        // ���ҽ� ��ο��� ĵ���� ������ �ε�
        GameObject missionCanvasPrefab = ResourceManager.Load<GameObject>(missionCanvasPrefabPath);
        CheckPrefabLoad(missionCanvasPrefab); // ������ �ε� ���� üũ

        // �������� ĵ���� ���� �� ObjectIndicator�� �߰�
        missionCanvas = Instantiate(missionCanvasPrefab, objectUIPanel);

        // objectUIPanel �ڵ� ���� (missionCanvas�� �ڽ����� ã��)
        if (objectUIPanel == null && missionCanvas != null)
        {
            objectUIPanel = missionCanvas.transform.Find("EnemyUIPanel");
            if (objectUIPanel == null)
            {
                Debug.LogError("EnemyUIPanel�� ã�� �� �����ϴ�. EnemyUIPanel�� �������ּ���.");
                return;
            }
        }

        // �ڽ� ������Ʈ���� �̹����� �ؽ�Ʈ�� ã�� ����
        missionCompletedImage = missionCanvas.transform.Find("EnemyUIPanel/MissionCompleted Image")?.GetComponent<Image>();
        missionCompletedText = missionCanvas.transform.Find("EnemyUIPanel/MissionCompleted Text")?.GetComponent<Text>();
        #region Image �� Text ������Ʈ ���� Ȯ��
        if (CheckImage(missionCompletedImage) == false) Debug.Log(":MissionCompleted Image");
        if (CheckText(missionCompletedText) == false) Debug.Log(":MissionCompleted Text");
        #endregion

        if (missionCompletedImage != null)
        {
            missionImageRect = missionCompletedImage.GetComponent<RectTransform>();
            missionCompletedImage.enabled = true;  // �ʱ⿡�� Ȱ��ȭ
        }

        if (missionCompletedText != null)
        {
            missionTextRect = missionCompletedText.GetComponent<RectTransform>();
            missionCompletedText.enabled = true;  // �ʱ⿡�� Ȱ��ȭ
        }
    }
    #endregion

    // ���� �̼��� UI Ȱ��ȭ
    public static void ShowNextMissionUI()
    {
        // ���� Ȱ��ȭ�� �̼��� �ִٸ� ��Ȱ��ȭ
        var currentIndicator = FindObjectOfType<MissionObjIndicator>();
        if (currentIndicator != null)
        {
            currentIndicator.HideUI(); // ���� �̼��� UI ��Ȱ��ȭ
        }

        // ���� �̼��� Indicator�� ã�� UI�� Ȱ��ȭ
        var nextMission = FindNextMissionIndicator();
        if (nextMission != null)
        {
            nextMission.ActivateUI(); // ���� �̼� UI Ȱ��ȭ
        }
        else
        {
            Debug.Log("�� �̻� ���� �̼��� �����ϴ�.");
        }
    }

    // ���� ���� �̼� �� ���� �̼��� ã�Ƽ� ��ȯ�ϴ� �Լ�
    private static MissionObjIndicator FindNextMissionIndicator()
    {
        // MissionManager�� ���� ���� �̼� ����Ʈ �� ���� �̼��� ã��
        var missionManager = GameManager.Mission;
        if (missionManager != null && missionManager.GetNextMission() != null)
        {
            var nextMissionObj = missionManager.GetNextMission().GetComponent<MissionObjIndicator>();
            return nextMissionObj;
        }

        return null; // ���� �̼��� ���� ���
    }

    // UI Ȱ��ȭ �޼���
    public void ActivateUI()
    {
        if (targetCanvas != null)
        {
            targetCanvas.SetActive(true); // UI Ȱ��ȭ
        }
        if (missionCanvas != null)
        {
            missionCanvas.SetActive(true); // �̼� ���� UI Ȱ��ȭ
        }
    }

    // ��ǥ Ž�� �� UI ������Ʈ
    void UpdateUI()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (player == null) return; // �÷��̾ ������ ����
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // �Ÿ� ��� UI Ȱ��ȭ �� ��Ȱ��ȭ ��ȯ
        if (distanceToPlayer > deactivationDistance)
        {
            targetImage.enabled = false;
            targetDistanceText.enabled = false;
            return;
        }
        else if (distanceToPlayer < activationDistance)
        {
            targetImage.enabled = true;
            targetDistanceText.enabled = true;
        }

        // ������Ʈ�� ��ġ�� ȭ�� ��ǥ�� ��ȯ
        Vector3 screenPos = mainCamera.WorldToScreenPoint(this.transform.position);

        // ������Ʈ�� ī�޶� �ڿ� ���� �� ȭ�� ������ ������ �ʵ��� ó��
        if (screenPos.z < 0)
        {
            screenPos.x = Screen.width - screenPos.x;
            screenPos.y = Screen.height - screenPos.y;
        }

        // UI �̹����� ��ġ�� ����
        imageRect.anchoredPosition = new Vector2(screenPos.x - Screen.width / 2, screenPos.y - Screen.height / 2);
        textRect.anchoredPosition = new Vector2(screenPos.x - Screen.width / 2, screenPos.y - Screen.height / 2);

        // ȭ�� ������ ������ ���, ��迡 ���̱�
        if (screenPos.z < 0 || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height)
        {
            Vector2 clampedPosition = new Vector2(
                Mathf.Clamp(screenPos.x, 0, Screen.width),
                Mathf.Clamp(screenPos.y, 0, Screen.height)
            );

            // UI ��ġ ���� (����, ������, ����, �Ʒ���)
            if (clampedPosition.x == 0) clampedPosition.x = 30; // ���� ��迡�� �ణ ��������
            else if (clampedPosition.x == Screen.width) clampedPosition.x = Screen.width - 30; // ������ ��迡�� �ణ ��������

            if (clampedPosition.y == 0) clampedPosition.y = 30; // ���� ��迡�� �ణ ��������
            else if (clampedPosition.y == Screen.height) clampedPosition.y = Screen.height - 30; // �Ʒ��� ��迡�� �ణ ��������

            imageRect.anchoredPosition = clampedPosition - new Vector2(Screen.width / 2, Screen.height / 2);
            textRect.anchoredPosition = clampedPosition - new Vector2(Screen.width / 2, Screen.height / 2);
        }

        // ȸ�� ó��: UI �̹����� ���� �÷��̾��� ������� ��ġ�� ����Ű���� ȸ��
        Vector3 directionToEnemy = (transform.position - player.position).normalized;
        Vector3 cameraForward = mainCamera.transform.forward;
        float angle = Mathf.Atan2(Vector3.Dot(cameraForward, Vector3.Cross(Vector3.up, directionToEnemy)), Vector3.Dot(cameraForward, directionToEnemy)) * Mathf.Rad2Deg;
        targetImage.transform.rotation = Quaternion.Euler(0, 0, angle + 90);

        // �Ÿ� �ؽ�Ʈ ������Ʈ
        targetDistanceText.text = $"{Mathf.Floor(distanceToPlayer)}m";
    }

    // ������Ʈ�� �Ϸ����� �� UI�� ǥ��
    void MarkObjectAsCompleted()
    {
        isMissionCompleted = true; // �̼� �Ϸ� ���·� ����

        if (missionCanvas != null)
        {
            if (missionCompletedImage == null)
            {
                Debug.LogError("missionCompletedImage�� null�Դϴ�. ������ ������ Ȯ���ϼ���.");
                return;
            }

            if (missionCompletedText == null)
            {
                Debug.LogError("missionCompletedText�� null�Դϴ�. ������ ������ Ȯ���ϼ���.");
                return;
            }

            if (targetCanvas != null)
            {
                Destroy(targetCanvas); // ��ǥ�� �Ϸ�Ǿ��� ��쿡�� ȭ��ǥ �̹��� �� �ؽ�Ʈ ����
                targetCanvas = null; // ������ null�� �����Ͽ� ���� ����
            }

            // ������Ʈ �Ϸ� ǥ��
            missionCompletedImage.color = Color.gray;
            missionCompletedText.text = "<b>- Mission Accomplished!</b>";

            // ���� �ð� �� �ؽ�Ʈ ����
            if (!hasConvertText)
            {
                hasConvertText = true;
                StartCoroutine(ChangeMissionTextAfterDelay(3f)); // 3�� �� �ؽ�Ʈ ����
            }
        }
    }

    IEnumerator ChangeMissionTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (isMissionCompleted)
        {
            missionCompletedText.text = "- Escape Here";
        }
    }

    // UI ��Ȱ��ȭ �޼��� �߰�
    public void HideUI()
    {
        if (targetCanvas != null)
        {
            targetCanvas.SetActive(false); // �̼��� �Ϸ�Ǿ��� �� UI�� ����ϴ�.
        }

        if (missionCanvas != null)
        {
            missionCanvas.SetActive(false); // �̼� ���� UI�� �����
        }
    }

    #region ���� üũ �޼���
    bool CheckPrefabLoad(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogError($"�������� {prefab} ��ο��� �ε��� �� �����ϴ�.");
            return false;
        }
        return true;
    }
    bool CheckImage(Image img)
    {
        if (img == null)
        {
            Debug.LogError("Image ������Ʈ�� ã�� �� ���ų� Image ������Ʈ�� �����ϴ�.");
            return false;
        }
        return true;
    }
    bool CheckText(Text text)
    {
        if (text == null)
        {
            Debug.LogError("Text ������Ʈ�� ã�� �� ���ų� Text ������Ʈ�� �����ϴ�.");
            return false;
        }
        return true;
    }
    #endregion
}
