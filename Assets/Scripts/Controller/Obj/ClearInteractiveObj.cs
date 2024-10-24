using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearInteractiveObj : InteractableObjExtand
{
    [Header("Main Camera")]
    [SerializeField] Camera mainCamera;

    [Header("Target(Object) UI")]
    [SerializeField] string targetCanvasPrefabPath = "Prefabs/UI/TargetIndicator/Target Enemy Canvas";
    [SerializeField] string missionCanvasPrefabPath = "Prefabs/UI/TargetIndicator/MissionFinalPoint";

    [Header("Target Distance Settings")]
    [SerializeField] float activationDistance = 500f;
    [SerializeField] float deactivationDistance = 500f;

    [Header("Object UI Settings")]
    [SerializeField] Transform objectUIPanel;

    [Header("Mission UI Settings")]
    [SerializeField] GameObject missionFailedPanel;
    [SerializeField] GameObject missionCompletedPanel;

    [SerializeField] private Transform targetObject; // Clear 대상 오브젝트
    [SerializeField] private GameObject specificObject; // 특정 오브젝트
    [SerializeField] private float clearDistance = 5f;

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

    private static ClearInteractiveObj currentIndicator;
    private Transform currentTarget;

    public void Start()
    {
        if (!interactable) return;

        // Load the UI before enabling components
        TargetLoadUI();
        MissionLoadUI();

        // 이미지와 텍스트 컴포넌트를 활성화
        if (targetImage != null)
        {
            targetImage.enabled = true;
        }

        if (targetDistanceText != null)
        {
            targetDistanceText.enabled = true;
        }

        if (missionCompletedImage != null)
        {
            missionCompletedImage.enabled = true;
        }

        if (missionCompletedText != null)
        {
            missionCompletedText.enabled = true;
        }

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
            currentIndicator = FindObjectOfType<ClearInteractiveObj>();
        }

        GameManager.Object.UpdateDelegate += UpdateUI;
    }

    public override void Interaction()
    {
        if (!interactable) return;

        // 특정 오브젝트 활성화
        MissionObjIndicator missionObjIndicator = FindObjectOfType<MissionObjIndicator>();
        if (missionObjIndicator != null)
        {
            missionObjIndicator.HideUI(); // UI 비활성화
        }

        targetCanvas.SetActive(false);
        MarkObjectAsCompleted();
        specificObject.SetActive(true); // 특정 오브젝트 활성화
        SpecitficObject specificObjComponent = specificObject.GetComponent<SpecitficObject>();
    }

    #region UI 프리팹 로드 및 추가 메서드
    void TargetLoadUI()
    {
        if (targetCanvas != null) return; // 이미 로드된 경우 로드하지 않음

        GameObject targetCanvasPrefab = Resources.Load<GameObject>(targetCanvasPrefabPath);
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

        GameObject missionCanvasPrefab = Resources.Load<GameObject>(missionCanvasPrefabPath);
        CheckPrefabLoad(missionCanvasPrefab);

        missionCanvas = Instantiate(missionCanvasPrefab, transform);
        objectUIPanel = missionCanvas.transform.Find("MissionFinalPointPanel");

        missionCompletedImage = missionCanvas.transform.Find("MissionFinalPointPanel/MissionCompleted Image")?.GetComponent<Image>();
        missionCompletedText = missionCanvas.transform.Find("MissionFinalPointPanel/MissionCompleted Text")?.GetComponent<Text>();

        if (missionCompletedImage != null)
        {
            missionImageRect = missionCompletedImage.GetComponent<RectTransform>();
            missionCompletedImage.enabled = false;
        }

        if (missionCompletedText != null)
        {
            missionTextRect = missionCompletedText.GetComponent<RectTransform>();
            missionCompletedText.enabled = false;
        }
    }
    #endregion

    public void ShowClearInteractiveIndicator(ClearInteractiveObj clearObj)
    {
        currentTarget = clearObj.transform;
        ActivateUI();
    }

    public void SetCurrentTarget(Transform target)
    {
        currentTarget = target;
    }

    public void ShowMissionUIForMission(ClearInteractiveObj missionObj)
    {
        if (currentIndicator != null)
        {
            currentIndicator.HideUI();
        }

        currentIndicator = missionObj.GetComponent<ClearInteractiveObj>();

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

        // 타겟 이미지가 삭제되었는지 확인
        if (targetImage == null || targetDistanceText == null)
        {
            Debug.LogWarning("타겟 이미지나 텍스트가 null입니다. UpdateUI를 중단합니다.");
            return; // null이면 더 이상 실행하지 않음

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

    public void MarkObjectAsCompleted()
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

            missionCompletedImage.color = Color.gray;
            missionCompletedText.text = "<b>- Escpae! Go to Exit! </b>";

            if (!hasConvertText)
            {
                hasConvertText = true;
                StartCoroutine(ChangeMissionTextAfterDelay(1f));
            }
        }
    }

    IEnumerator ChangeMissionTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
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
