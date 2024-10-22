using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionObjIndicator : MonoBehaviour
{
    [Header("Main Camera")]
    [SerializeField] Camera mainCamera;

    [Header("Target(Object) UI")]
    [SerializeField] string targetCanvasPrefabPath = "Prefabs/UI/TargetIndicator/Target Enemy Canvas";
    [SerializeField] string missionCanvasPrefabPath = "Prefabs/UI/TargetIndicator/Mission Status Canvas";

    [Header("Target Distance Settings")]
    [SerializeField] float activationDistance = 500f;
    [SerializeField] float deactivationDistance = 500f;

    [Header("Object UI Settings")]
    [SerializeField] Transform objectUIPanel;

    [Header("Mission UI Settings")]
    [SerializeField] GameObject missionFailedPanel;
    [SerializeField] GameObject missionCompletedPanel;

    private Transform player;

    public bool isMissionCompleted = false;
    public bool hasConvertText = false;

    private Image targetImage;
    private Text targetDistanceText;
    private Image missionCompletedImage;
    private Text missionCompletedText;

    private RectTransform imageRect;
    private RectTransform textRect;
    private RectTransform missionImageRect;
    private RectTransform missionTextRect;

    private GameObject targetCanvas;
    private GameObject missionCanvas;

    private static MissionObjIndicator currentIndicator;
    private Transform currentTarget;

    public void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("Main Camera를 찾을 수 없습니다. 메인 카메라를 설정해주세요.");
                return;
            }
        }

        if (currentIndicator == null)
        {
            currentIndicator = FindObjectOfType<MissionObjIndicator>();
        }

        TargetLoadUI();
        MissionLoadUI();

        GameManager.Object.UpdateDelegate += UpdateUI;
    }

    #region UI 프리팹 로드 및 추가 메서드
    void TargetLoadUI()
    {
        if (targetCanvas != null) return; // 이미 로드된 경우 로드하지 않음

        GameObject targetCanvasPrefab = ResourceManager.Load<GameObject>(targetCanvasPrefabPath);
        CheckPrefabLoad(targetCanvasPrefab);

        targetCanvas = Instantiate(targetCanvasPrefab, transform);
        targetImage = targetCanvas.transform.Find("Position Image")?.GetComponent<Image>();
        targetDistanceText = targetCanvas.transform.Find("Distance Text")?.GetComponent<Text>();

        if (targetImage != null)
        {
            imageRect = targetImage.GetComponent<RectTransform>();
            targetImage.enabled = false;
        }

        if (targetDistanceText != null)
        {
            textRect = targetDistanceText.GetComponent<RectTransform>();
            targetDistanceText.enabled = false;
        }
    }

    void MissionLoadUI()
    {
        if (missionCanvas != null) return; // 이미 로드된 경우 로드하지 않음

        GameObject missionCanvasPrefab = ResourceManager.Load<GameObject>(missionCanvasPrefabPath);
        CheckPrefabLoad(missionCanvasPrefab);

        missionCanvas = Instantiate(missionCanvasPrefab, objectUIPanel);
        objectUIPanel = missionCanvas.transform.Find("EnemyUIPanel");

        missionCompletedImage = missionCanvas.transform.Find("EnemyUIPanel/MissionCompleted Image")?.GetComponent<Image>();
        missionCompletedText = missionCanvas.transform.Find("EnemyUIPanel/MissionCompleted Text")?.GetComponent<Text>();

        if (missionCompletedImage != null)
        {
            missionImageRect = missionCompletedImage.GetComponent<RectTransform>();
            missionCompletedImage.enabled = true;
        }

        if (missionCompletedText != null)
        {
            missionTextRect = missionCompletedText.GetComponent<RectTransform>();
            missionCompletedText.enabled = true;
        }
    }
    #endregion

    public void ShowClearInteractiveIndicator(ClearInteractiveObj clearObj)
    {
        if (currentIndicator != null)
        {
            currentIndicator.HideUI();
        }

        currentIndicator = clearObj.GetComponent<MissionObjIndicator>();

        if (currentIndicator != null)
        {
            currentIndicator.ActivateUI();
            currentIndicator.SetCurrentTarget(clearObj.transform);
        }
        else
        {
            Debug.LogError("MissionObjIndicator를 ClearInteractiveObj에서 찾을 수 없습니다.");
        }
    }

    public void SetCurrentTarget(Transform target)
    {
        currentTarget = target;
    }

    public void ShowMissionUIForMission(StageInteractiveObj missionObj)
    {
        if (currentIndicator != null)
        {
            currentIndicator.HideUI();
        }

        currentIndicator = missionObj.GetComponent<MissionObjIndicator>();

        if (currentIndicator != null)
        {
            currentIndicator.ActivateUI();
            currentIndicator.SetCurrentTarget(missionObj.transform);
        }
    }

    public void ActivateUI()
    {
        if (targetCanvas != null)
        {
            targetCanvas.SetActive(true);
        }

        if (missionCanvas != null)
        {
            missionCanvas.SetActive(true);
        }
    }

    public void HideUI()
    {
        if (targetCanvas != null)
        {
            targetCanvas.SetActive(false);
        }

        if (missionCanvas != null)
        {
            missionCanvas.SetActive(false);
        }
    }

    void UpdateUI()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (player == null) return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

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

        Vector3 screenPos = mainCamera.WorldToScreenPoint(this.transform.position);

        if (screenPos.z < 0)
        {
            screenPos.x = Screen.width - screenPos.x;
            screenPos.y = Screen.height - screenPos.y;
        }

        imageRect.anchoredPosition = new Vector2(screenPos.x - Screen.width / 2, screenPos.y - Screen.height / 2);
        textRect.anchoredPosition = new Vector2(screenPos.x - Screen.width / 2, screenPos.y - Screen.height / 2);

        if (screenPos.z < 0 || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height)
        {
            Vector2 clampedPosition = new Vector2(
                Mathf.Clamp(screenPos.x, 0, Screen.width),
                Mathf.Clamp(screenPos.y, 0, Screen.height)
            );

            if (clampedPosition.x == 0) clampedPosition.x = 30;
            else if (clampedPosition.x == Screen.width) clampedPosition.x = Screen.width - 30;

            if (clampedPosition.y == 0) clampedPosition.y = 30;
            else if (clampedPosition.y == Screen.height) clampedPosition.y = Screen.height - 30;

            imageRect.anchoredPosition = clampedPosition - new Vector2(Screen.width / 2, Screen.height / 2);
            textRect.anchoredPosition = clampedPosition - new Vector2(Screen.width / 2, Screen.height / 2);
        }

        Vector3 directionToEnemy = (transform.position - player.position).normalized;
        Vector3 cameraForward = mainCamera.transform.forward;
        float angle = Mathf.Atan2(Vector3.Dot(cameraForward, Vector3.Cross(Vector3.up, directionToEnemy)), Vector3.Dot(cameraForward, directionToEnemy)) * Mathf.Rad2Deg;
        targetImage.transform.rotation = Quaternion.Euler(0, 0, angle + 90);

        targetDistanceText.text = $"{Mathf.Floor(distanceToPlayer)}m";
    }

    void MarkObjectAsCompleted()
    {
        isMissionCompleted = true;

        if (missionCanvas != null)
        {
            if (missionCompletedImage == null)
            {
                Debug.LogError("missionCompletedImage가 null입니다.");
                return;
            }

            if (missionCompletedText == null)
            {
                Debug.LogError("missionCompletedText가 null입니다.");
                return;
            }

            if (targetCanvas != null)
            {
                Destroy(targetCanvas);
                targetCanvas = null;
            }

            missionCompletedImage.color = Color.gray;
            missionCompletedText.text = "<b>- Mission Accomplished!</b>";

            if (!hasConvertText)
            {
                hasConvertText = true;
                StartCoroutine(ChangeMissionTextAfterDelay(3f));
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

    #region 에러 체크 메서드
    bool CheckPrefabLoad(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogError($"프리팹을 {prefab} 경로에서 로드할 수 없습니다.");
            return false;
        }
        return true;
    }

    bool CheckImage(Image img)
    {
        if (img == null)
        {
            Debug.LogError("Image 오브젝트를 찾을 수 없거나 Image 컴포넌트가 없습니다.");
            return false;
        }
        return true;
    }

    bool CheckText(Text text)
    {
        if (text == null)
        {
            Debug.LogError("Text 오브젝트를 찾을 수 없거나 Text 컴포넌트가 없습니다.");
            return false;
        }
        return true;
    }
    #endregion
}
