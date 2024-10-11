using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyInteractive : InteractableObjExtand
{
    EnemyStatus _status;

    [Header("Detection UI")]
    [SerializeField] GameObject weakDetectionUI; // '?', 플레이어 발견
    [SerializeField] GameObject strongDetectionUI; // '!', 플레이어 완전히 탐지

    [Header("Stealth UI")]
    [SerializeField] GameObject stealthUI;

    [Header("UI Settings")]
    [SerializeField] Vector3 uiOffset = new Vector3(0, 2f, 0); // 머리 위로 UI를 올리기 위한 오프셋 값
    [SerializeField] float stealthAngleThreshold = 60f; // 플레이어가 적의 등 뒤에 있어야 하는 각도
    [SerializeField] float maxStealthDistance = 2.5f; // 플레이어와 적 사이의 최대 스텔스 상호작용 거리

    // Start is called before the first frame update
    void Start()
    {
        _status = GetComponent<EnemyStatus>();
    }
    private void Update()
    {
        //if(_status.player == null) { return; }
        //Debug.Log(_status.player.transform.position);

        UpdateUI();
    }

    void UpdateUI()
    {
        if (_status.IsAlive == true && _status.executable == true && _status.executing == false && IsPlayerInStealthRange())
        {
            ui_Show = true;
            UpdateUIPosition(stealthUI);
            UIShow(stealthUI);
        }
        else
        {
            UIHide(stealthUI);
        }

        if (_status.weakDetecting == true)
        {
            ui_Show = true;
            UpdateUIPosition(weakDetectionUI);
            UIShow(weakDetectionUI);
        }
        else
        {
            UIHide(weakDetectionUI);
        }
        
        if (_status.strongDetecting == true)
        {
            ui_Show = true;
            UpdateUIPosition(strongDetectionUI);
            UIShow(strongDetectionUI);
        }
        else
        {
            UIHide(strongDetectionUI);
        }

        if (!ui_Show || _status.IsAlive == false)
        {
            UIHide(stealthUI);
            UIHide(weakDetectionUI);
            UIHide(strongDetectionUI);
        }
    }

    public override void Interaction()
    {
        if (_status.IsAlive == false || _status.executable == false || !IsPlayerInStealthRange())
            return;

        _status.Hp = -1;
        base.Interaction();
        ui_Show = false;

        _status.executing = true; // 처형당하는 중

        // 현재 플레이어와 이 오브젝트의 위치를 사용해 y축 회전만 적용
        Vector3 targetPosition = this.transform.position;
        targetPosition.y = _status.player.transform.position.y; // y 축 고정

        // LookAt 대신 y축만을 고려한 회전 적용
        _status.player.transform.LookAt(targetPosition);
        this.transform.rotation = Quaternion.LookRotation(_status.player.transform.forward);

        Destroy(this.GetComponent<CapsuleCollider>());
    }

    void UIShow(GameObject obj)
    {
        //if (ui_Show == false || _status.IsAlive == false)
        if (obj != null && !obj.activeSelf)
        {
            obj.SetActive(true);  // UI가 꺼져 있으면 켜기
            Debug.Log($"UI {obj.name} activated");
        }

    }
    void UIHide(GameObject obj)
    {
        if (obj != null && obj.activeSelf)
        {
            obj.SetActive(false);  // UI가 켜져 있으면 끄기
            Debug.Log($"UI {obj.name} deactivated");
        }

    }

    void UpdateUIPosition(GameObject obj) // UI 적의 머리 위로 이동
    {
        if (_status.player == null)
            return;
        if (obj != null)
        {
            obj.transform.position = _status.transform.position + uiOffset;
        }
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
}
