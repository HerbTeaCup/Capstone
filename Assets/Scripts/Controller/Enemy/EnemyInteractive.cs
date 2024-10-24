using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyInteractive : InteractableObjExtand
{
    public EnemyStatus _status;

    [Header("Detection UI")]
    [SerializeField] GameObject weakDetectionUIPrefab;  // 인스턴스된 '?' UI
    [SerializeField] GameObject strongDetectionUIPrefab;  // 인스턴스된 '!' UI

    [Header("Stealth UI")]
    [SerializeField] GameObject stealthUIPrefab;  // 인스턴스된 스텔스 UI

    [Header("UI Settings")]
    [SerializeField] Vector3 uiOffset = new Vector3(0, 2f, 0); // 머리 위로 UI를 올리기 위한 오프셋 값
    [SerializeField] float stealthAngleThreshold = 60f; // 플레이어가 적의 등 뒤에 있어야 하는 각도
    [SerializeField] float maxStealthDistance = 2.5f; // 플레이어와 적 사이의 최대 스텔스 상호작용 거리

    // 복제된 UI
    GameObject currentStealthUI = null; // 복제된 암살 UI
    GameObject currentWeakDetectionUI = null; // 복제된 약한 탐지 UI
    GameObject currentStrongDetectionUI = null; // 복제된 강한 탐지 UI

    private void Start()
    {
        _status = GetComponent<EnemyStatus>();
    }

    private void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        if (!_status.IsAlive)
        {
            // 적이 죽었을 때 모든 UI 비활성화 및 삭제
            DestroyAllUI();
            return;
        }

        // 1. Stealth UI
        if (_status.IsAlive && _status.executable && !_status.executing && IsPlayerInStealthRange())
        {
            if (currentStealthUI == null) // Stealth UI가 아직 생성되지 않았을 때만 생성
            {
                currentStealthUI = Instantiate(stealthUIPrefab);
                UIShow(currentStealthUI);
            }
            UpdateUIPosition(currentStealthUI);
        }
        else
        {
            if (currentStealthUI != null) // 상호작용 범위를 벗어나면 UI 삭제
            {
                UIHide(currentStealthUI);
                currentStealthUI = null;
            }
        }

        // 2. Weak Detection UI
        if (_status.weakDetecting)
        {
            if (currentWeakDetectionUI == null) // Weak Detection UI가 없으면 생성
            {
                currentWeakDetectionUI = Instantiate(weakDetectionUIPrefab);
                UIShow(currentWeakDetectionUI);
            }
            UpdateUIPosition(currentWeakDetectionUI);
        }
        else
        {
            if (currentWeakDetectionUI != null) // UI 숨김 및 삭제
            {
                UIHide(currentWeakDetectionUI);
                currentWeakDetectionUI = null;
            }
        }

        // 3. Strong Detection UI
        if (_status.strongDetecting)
        {
            if (currentStrongDetectionUI == null) // Strong Detection UI가 없으면 생성
            {
                currentStrongDetectionUI = Instantiate(strongDetectionUIPrefab);
                UIShow(currentStrongDetectionUI);
            }
            UpdateUIPosition(currentStrongDetectionUI);
        }
        else
        {
            if (currentStrongDetectionUI != null) // UI 숨김 및 삭제
            {
                UIHide(currentStrongDetectionUI);
                currentStrongDetectionUI = null;
            }
        }
    }

    void UIShow(GameObject obj)
    {
        if (obj != null && !obj.activeSelf)
        {
            obj.SetActive(true);  // UI가 꺼져 있으면 켜기
            // Debug.Log($"UI {obj.name} activated");
        }
    }

    void UIHide(GameObject obj)
    {
        if (obj != null && obj.activeSelf)
        {
            obj.SetActive(false);  // UI가 켜져 있으면 끄기
            Destroy(obj);
            // Debug.Log($"UI {obj.name} deactivated");
        }
    }

    void UpdateUIPosition(GameObject obj) // UI 적의 머리 위로 이동
    {
        /*        if (_status.player == null)
                    return;*/
        
        /*if (obj != null)
        {
            obj.transform.position = this.transform.position + uiOffset;
        }*/

        if (obj != null)
        {
            // 카메라의 위치와 방향을 기준으로 UI 위치 업데이트
            Vector3 uiPosition = this.transform.position + uiOffset;
            uiPosition.y += Mathf.Sin(Time.time) * 0.1f; // 약간의 흔들림 효과
            obj.transform.position = uiPosition;

            // UI를 반전시켜 보이게 함
            obj.transform.localScale = new Vector3(-1f, 1f, 1f); // X축 반전

            // 카메라의 방향을 고려하여 UI가 항상 카메라를 바라보도록 설정
            obj.transform.LookAt(Camera.main.transform);
        }
    }

    // 암살 UI, 탐지 UI 모두 삭제하는 메서드 (적이 죽을 때)
    void DestroyAllUI()
    {
        UIHide(currentStealthUI);
        UIHide(currentWeakDetectionUI);
        UIHide(currentStrongDetectionUI);
        currentStealthUI = null;
        currentWeakDetectionUI = null;
        currentStrongDetectionUI = null;
    }

    bool IsPlayerInStealthRange()
    {
        if (_status.player == null) return false;

        // 1. 플레이어와 적 사이의 거리 계산
        float distanceToPlayer = Vector3.Distance(_status.player.transform.position, transform.position);
        if (distanceToPlayer > maxStealthDistance) return false; // 거리가 너무 멀면 false

        // 2. 플레이어가 적의 뒤에 있는지 확인
        Vector3 toPlayer = _status.player.transform.position - transform.position; // 적에서 플레이어까지의 벡터
        toPlayer.y = 0; // Y축 값 무시 (수평 방향만 계산)

        Vector3 forward = transform.forward; // 적의 전방 방향
        forward.y = 0; // Y축 값 무시 (수평 방향만 계산)

        float angle = Vector3.Angle(forward, toPlayer); // 두 벡터 사이의 각도 계산

        // 플레이어가 적의 등 뒤에 있는지 확인 (stealthAngleThreshold 내에 있을 경우)
        return angle > 180f - stealthAngleThreshold / 2f && angle < 180f + stealthAngleThreshold / 2f;
    }

    public override void Interaction()
    {
        if (!_status.IsAlive || !_status.executable || !IsPlayerInStealthRange())
            return;

        _status.Hp = -1;
        base.Interaction();
        ui_Show = false;

        _status.executing = true; // 처형당하는 중

        // 현재 플레이어와 이 오브젝트의 위치를 사용해 y축 회전만 적용
        Vector3 targetPosition = transform.position;
        targetPosition.y = _status.player.transform.position.y; // y 축 고정

        // LookAt 대신 y축만을 고려한 회전 적용
        _status.player.transform.LookAt(targetPosition);
        transform.rotation = Quaternion.LookRotation(_status.player.transform.forward);

        Destroy(GetComponent<CapsuleCollider>());
    }
}
