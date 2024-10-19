using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyIndicator : MonoBehaviour
{
    [Header("Main Camera")]
    [SerializeField] Camera mainCamera; // 메인 카메라

    [Header("Target(Enemy) UI")]
    [SerializeField] string targetCanvasPrefabPath = "Prefabs/UI/TargetIndicator/Target Enemy UI";  // 캔버스 프리팹 경로

    [Header("Target Distance Settings")]
    [SerializeField] float activationDistance = 20f; // 목표 활성화 거리
    [SerializeField] float deactivationDistance = 20f; // 목표 비활성화 거리

    EnemyStatus _status;
    private Transform player; // 플레이어의 Transform
    private Image targetImage;  // 동적 로드 UI 이미지
    private Text targetDistanceText;  // 동적 로드 거리 텍스트
    private RectTransform imageRect; // UI 이미지의 RectTransform
    private RectTransform textRect; // UI 텍스트의 RectTransform

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
        LoadUI();
        GameManager.Enemy.UpdateDelegate += UpdateUI;
    }

    // UI 프리팹 로드 및 추가
    void LoadUI()
    {
        // 리소스 경로에서 캔버스 프리팹 로드
        GameObject targetCanvasPrefab = ResourceManager.Load<GameObject>(targetCanvasPrefabPath);
        if (targetCanvasPrefab == null)
        {
            Debug.LogError($"Target Canvas 프리팹을 {targetCanvasPrefabPath} 경로에서 로드할 수 없습니다.");
            return;
        }

        // 동적으로 캔버스 생성 및 EnemyIndicator에 추가
        GameObject targetCanvas = Instantiate(targetCanvasPrefab, transform);

        // 자식 오브젝트에서 이미지와 텍스트를 찾아 설정
        targetImage = targetCanvas.transform.Find("Position Image")?.GetComponent<Image>();
        if (targetImage == null)
        {
            Debug.LogError("Position Image 오브젝트를 찾을 수 없거나 Image 컴포넌트가 없습니다.");
        }

        targetDistanceText = targetCanvas.transform.Find("Distance Text")?.GetComponent<Text>();
        if (targetDistanceText == null)
        {
            Debug.LogError("Distance Text 오브젝트를 찾을 수 없거나 Text 컴포넌트가 없습니다.");
        }

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

    // 목표 탐지 및 UI 업데이트
    void UpdateUI()
    {
        // 적의 상태 업데이트
        if (_status.IsAlive == false || _status.executing) { return; }

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
        targetImage.transform.rotation = Quaternion.Euler(0, 0, angle+90);


        // 거리 텍스트 업데이트
        targetDistanceText.text = $"{Mathf.Floor(distanceToPlayer)}m";
    }
}
