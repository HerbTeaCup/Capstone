using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyIndicator : MonoBehaviour
{
    EnemyStatus _status;

    [Header("Main Camera")]
    [SerializeField] Camera mainCamera; // 메인 카메라

    [Header("Target(Enemy) UI")]
    [SerializeField] string targetCanvasPrefabPath = "Prefabs/UI/TargetIndicator/Target Enemy UI";  // 캔버스 프리팹 경로

    [Header("Target Distance Settings")]
    [SerializeField] float activationDistance = 10f; // 목표 활성화 거리
    [SerializeField] float deactivationDistance = 20f; // 목표 비활성화 거리

    private Image targetImage;  // 동적 로드 UI 이미지
    private Text targetDistanceText;  // 동적 로드 거리 텍스트
    private RectTransform imageRect; // UI 이미지의 RectTransform
    private RectTransform canvasRect; // UI 캔버스의 RectTransform

    void Start()
    {
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
            canvasRect = targetImage.canvas.GetComponent<RectTransform>();
            targetImage.enabled = false;  // 초기에는 비활성화
        }

        if (targetDistanceText != null)
        {
            targetDistanceText.enabled = false;  // 초기에는 비활성화
        }
    }

    // 목표 탐지 및 UI 업데이트
    void UpdateUI()
    {
        // 적의 상태 업데이트
        if (_status.IsAlive == false || _status.executing) { return; }

        float distanceToPlayer;
        bool foundPlayer = TargetSearching(out distanceToPlayer);

        if (foundPlayer)
        {
            UpdateTargetUI(distanceToPlayer);
        }
        else
        {
            if (targetImage != null)
                targetImage.enabled = false;

            if (targetDistanceText != null)
                targetDistanceText.enabled = false;
        }
    }

    // 목표 탐지
    bool TargetSearching(out float distanceToPlayer)
    {
        Collider[] temp = Physics.OverlapSphere(this.transform.position, _status.searchRadius, LayerMask.GetMask("Player"));
        distanceToPlayer = Mathf.Infinity;
        _status.player = null;

        if (temp.Length == 0) { return false; }

        foreach (Collider item in temp)
        {
            if (item.CompareTag("Player"))
            {
                _status.player = item.transform;
                break;
            }
        }

        if (_status.player == null) { return false; }

        distanceToPlayer = Vector3.Distance(this.transform.position, _status.player.position);
        return true;
    }

    #region 목표 UI 업데이트
    // UI를 적의 위치로 업데이트
    void UpdateTargetUI(float distanceToPlayer)
    {
        if (targetImage == null || targetDistanceText == null) return;

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
        Vector3 screenPos = mainCamera.WorldToScreenPoint(_status.player.position);

        // 적이 카메라 뒤에 있을 때 화면 밖으로 보내지 않도록 처리
        if (screenPos.z < 0)
        {
            screenPos.x = Screen.width - screenPos.x;
            screenPos.y = Screen.height - screenPos.y;
        }

        // UI 이미지의 위치를 갱신 (화면 좌표계 -> 캔버스 좌표계로 변환)
        imageRect.anchoredPosition = new Vector2(screenPos.x - Screen.width / 2, screenPos.y - Screen.height / 2);

        // UI 이미지가 적의 방향을 표시하도록 회전
        Vector3 directionToEnemy = (_status.player.position - mainCamera.transform.position).normalized;
        float angle = Mathf.Atan2(directionToEnemy.x, directionToEnemy.z) * Mathf.Rad2Deg;
        imageRect.rotation = Quaternion.Euler(0, 0, -angle);

        // 거리 텍스트를 이미지와 함께 업데이트
        UpdateDistanceText(distanceToPlayer, screenPos);
    }

    // 거리 텍스트를 업데이트
    void UpdateDistanceText(float distanceToPlayer, Vector3 screenPos)
    {
        if (targetDistanceText == null) return;

        targetDistanceText.text = $"{Mathf.Floor(distanceToPlayer)}m";  // 거리를 미터 단위로 표시
        targetDistanceText.enabled = true;  // UI 활성화 시 거리 텍스트도 활성화

        // 거리 텍스트의 위치도 화살표 이미지와 동일하게 업데이트
        targetDistanceText.rectTransform.anchoredPosition = new Vector2(screenPos.x - Screen.width / 2, screenPos.y - Screen.height / 2);
    }
    #endregion
}
