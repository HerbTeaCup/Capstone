using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyIndicator : MonoBehaviour
{
    EnemyStatus _status;

    [Header("Main Camera")]
    [SerializeField] Camera mainCamera; // 메인 카메라

    [Header("Target(Enemy) UI")]
    [SerializeField] string targetCanvasPrefabPath = "Prefabs/UI/TargetIndicator/Target Enemy Canvas";  // 캔버스 프리팹 경로
    [SerializeField] string missionCanvasPrefabPath = "Prefabs/UI/TargetIndicator/Mission Canvas";  // 캔버스 프리팹 경로

    [Header("Target Distance Settings")]
    [SerializeField] float activationDistance = 20f; // 목표 활성화 거리
    [SerializeField] float deactivationDistance = 20f; // 목표 비활성화 거리

    [Header("Enemy UI Settings")]
    [SerializeField] Transform enemyUIPanel; // 모든 적의 UI를 표시할 패널

    private Transform player; // 플레이어의 Transform

    public bool isMissionCompleted = false;
    public bool hasConvertText = false;

    // 동적 로드 UI
    private Image targetImage;  // 목표 위치 가시성 이미지
    private Text targetDistanceText;  // 목표 위치 텍스트
    private Image missionCompletedImage; // 미션 완수 이미지
    private Text missionCompletedText; // 미션 완수 텍스트

    private RectTransform imageRect; // UI 이미지의 RectTransform
    private RectTransform textRect; // UI 텍스트의 RectTransform
    private RectTransform missionImageRect; // UI 이미지의 RectTransform
    private RectTransform missionTextRect; // UI 텍스트의 RectTransform

    private GameObject targetCanvas; // 목표 위치 Canvas
    private GameObject missionCanvas; // 미션 Canvas

    void Start()
    {
        // Main Camera 자동 설정
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("Main Camera를 찾을 수 없습니다. 메인 카메라를 설정해주세요.");
                return;
            }
        }

        _status = GetComponent<EnemyStatus>();

        TargetLoadUI();
        MissionLoadUI();

        GameManager.Enemy.UpdateDelegate += UpdateUI;
    }


    #region UI 프리팹 로드 및 추가 메서드
    // UI 프리팹 로드 및 추가
    void TargetLoadUI()
    {
        // 리소스 경로에서 캔버스 프리팹 로드
        GameObject targetCanvasPrefab = ResourceManager.Load<GameObject>(targetCanvasPrefabPath);
        CheckPrefabLoad(targetCanvasPrefab); // 프리팹 로드 에러 체크

        // 동적으로 캔버스 생성 및 EnemyIndicator에 추가
        targetCanvas = Instantiate(targetCanvasPrefab, transform);

        // 자식 오브젝트에서 이미지와 텍스트를 찾아 설정
        targetImage = targetCanvas.transform.Find("Position Image")?.GetComponent<Image>();
        targetDistanceText = targetCanvas.transform.Find("Distance Text")?.GetComponent<Text>();
        #region Image 및 Text 컴포넌트 에러 확인
        if (CheckImage(targetImage) == false) Debug.Log(":Position Image");
        if (CheckText(targetDistanceText) == false) Debug.Log(":Distance Text");
        #endregion


        if (targetImage != null)
        {
            imageRect = targetImage.GetComponent<RectTransform>();
            targetImage.enabled = false;  // 초기에는 비활성화
        }

        if (targetDistanceText != null)
        {
            textRect = targetDistanceText.GetComponent<RectTransform>();
            targetDistanceText.enabled = false;  // 초기에는 비활성화
        }
    }
    void MissionLoadUI()
    {
        // 리소스 경로에서 캔버스 프리팹 로드
        GameObject missionCanvasPrefab = ResourceManager.Load<GameObject>(missionCanvasPrefabPath);
        CheckPrefabLoad(missionCanvasPrefab); // 프리팹 로드 에러 체크

        // 동적으로 캔버스 생성 및 EnemyIndicator에 추가
        missionCanvas = Instantiate(missionCanvasPrefab, enemyUIPanel);

        // enemyUIPanel 자동 설정 (missionCanvas의 자식으로 찾기)
        if (enemyUIPanel == null && missionCanvas != null)
        {
            enemyUIPanel = missionCanvas.transform.Find("EnemyUIPanel");
            if (enemyUIPanel == null)
            {
                Debug.LogError("EnemyUIPanel을 찾을 수 없습니다. EnemyUIPanel을 설정해주세요.");
                return;
            }
        }

        // 자식 오브젝트에서 이미지와 텍스트를 찾아 설정
        missionCompletedImage = missionCanvas.transform.Find("EnemyUIPanel/MissionCompleted Image")?.GetComponent<Image>();
        missionCompletedText = missionCanvas.transform.Find("EnemyUIPanel/MissionCompleted Text")?.GetComponent<Text>();
        #region Image 및 Text 컴포넌트 에러 확인
        if (CheckImage(missionCompletedImage) == false) Debug.Log(":MissionCompleted Image");
        if (CheckText(missionCompletedText) == false) Debug.Log(":MissionCompleted Text");
        #endregion

        if (missionCompletedImage != null)
        {
            missionImageRect = missionCompletedImage.GetComponent<RectTransform>();
            missionCompletedImage.enabled = true;  // 초기에는 활성화
        }

        if (missionCompletedText != null)
        {
            missionTextRect = missionCompletedText.GetComponent<RectTransform>();
            missionCompletedText.enabled = true;  // 초기에는 활성화
        }
    }
    #endregion 

    // 목표 탐지 및 UI 업데이트
    void UpdateUI()
    {
        // 적의 상태 업데이트
        if (_status.IsAlive == false || _status.executing)
        {
            if (!isMissionCompleted)
            {
                MarkEnemyAsDefeated();
            }
            return;
        }

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (player == null) return; // 플레이어가 없으면 리턴
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 거리 기반 UI 활성화 및 비활성화 전환
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

        // 적의 위치를 화면 좌표로 변환
        Vector3 screenPos = mainCamera.WorldToScreenPoint(this.transform.position);

        // 적이 카메라 뒤에 있을 때 화면 밖으로 보내지 않도록 처리
        if (screenPos.z < 0)
        {
            screenPos.x = Screen.width - screenPos.x;
            screenPos.y = Screen.height - screenPos.y;
        }

        // UI 이미지의 위치를 갱신
        imageRect.anchoredPosition = new Vector2(screenPos.x - Screen.width / 2, screenPos.y - Screen.height / 2);
        textRect.anchoredPosition = new Vector2(screenPos.x - Screen.width / 2, screenPos.y - Screen.height / 2);

        // 화면 밖으로 나갔을 경우, 경계에 붙이기
        if (screenPos.z < 0 || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height)
        {
            Vector2 clampedPosition = new Vector2(
                Mathf.Clamp(screenPos.x, 0, Screen.width),
                Mathf.Clamp(screenPos.y, 0, Screen.height)
            );

            // UI 위치 조정 (왼쪽, 오른쪽, 위쪽, 아래쪽)
            if (clampedPosition.x == 0) clampedPosition.x = 30; // 왼쪽 경계에서 약간 떨어지게
            else if (clampedPosition.x == Screen.width) clampedPosition.x = Screen.width - 30; // 오른쪽 경계에서 약간 떨어지게

            if (clampedPosition.y == 0) clampedPosition.y = 30; // 위쪽 경계에서 약간 떨어지게
            else if (clampedPosition.y == Screen.height) clampedPosition.y = Screen.height - 30; // 아래쪽 경계에서 약간 떨어지게

            imageRect.anchoredPosition = clampedPosition - new Vector2(Screen.width / 2, Screen.height / 2);
            textRect.anchoredPosition = clampedPosition - new Vector2(Screen.width / 2, Screen.height / 2);
        }

        // 회전 처리: UI 이미지가 적과 플레이어의 상대적인 위치를 가리키도록 회전
        Vector3 directionToEnemy = (transform.position - player.position).normalized;
        Vector3 cameraForward = mainCamera.transform.forward;
        float angle = Mathf.Atan2(Vector3.Dot(cameraForward, Vector3.Cross(Vector3.up, directionToEnemy)), Vector3.Dot(cameraForward, directionToEnemy)) * Mathf.Rad2Deg;
        targetImage.transform.rotation = Quaternion.Euler(0, 0, angle + 90);


        // 거리 텍스트 업데이트
        targetDistanceText.text = $"{Mathf.Floor(distanceToPlayer)}m";
    }

    // 적을 잡았을 때 UI에 표시
    void MarkEnemyAsDefeated()
    {
        isMissionCompleted = true; // 미션 완료 상태로 설정

        if (missionCanvas != null)
        {
            if (missionCompletedImage == null)
            {
                Debug.LogError("missionCompletedImage가 null입니다. 프리팹 구조를 확인하세요.");
                return;
            }

            if (missionCompletedText == null)
            {
                Debug.LogError("missionCompletedText가 null입니다. 프리팹 구조를 확인하세요.");
                return;
            }

            if (targetCanvas != null)
            {
                Destroy(targetCanvas); // 목표가 죽었을 경우에는 화살표 이미지 및 텍스트 제거
                targetCanvas = null; // 참조를 null로 설정하여 접근 방지
            }

            // 적을 죽였다는 표시
            missionCompletedImage.color = Color.gray;
            missionCompletedText.text = "<b>- Mission Accomplished!</b>";

            // 일정 시간 후 텍스트 변경
            if (!hasConvertText)
            {
                hasConvertText = true;
                StartCoroutine(ChangeMissionTextAfterDelay(3f)); // 3초 후 텍스트 변경
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