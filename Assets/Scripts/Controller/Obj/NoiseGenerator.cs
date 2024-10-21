using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGenerator : InteractableObjExtand
{
    AudioSource _audio;
    GameObject player;

    [Header("Sound")]
    [SerializeField] AudioClip Sound;

    [Header("Setting")]
    [SerializeField] float _workingTime = 8f; // 상호작용이 완료되기까지의 시간
    float _timeDelta = 0f;

    [Header("UI")]
    [SerializeField] GameObject interactionUIPrefab;
    [SerializeField] Vector3 uiOffset = new Vector3(0, 0, 0);

    GameObject interactionUI = null;

    private void Start()
    {
        _audio = this.GetComponent<AudioSource>();
        if (_audio == null)
        {
            _audio = gameObject.AddComponent<AudioSource>();
            Debug.Log("AudioSource가 없어서 자동으로 추가되었습니다.");
        }

        player = GameObject.FindWithTag("Player");

        if (Sound == null)
        {
            Sound = Resources.Load<AudioClip>("ElevatorError");
            if (Sound == null)
            {
                Debug.LogError("Sound 파일을 찾을 수 없습니다. 경로를 확인하세요.");
            }
            else
            {
                Debug.Log("Sound 파일이 정상적으로 로드되었습니다.");
            }
        }

        if (interactionUIPrefab == null)
        {
            interactionUIPrefab = Resources.Load<GameObject>("Prefabs/UI/Object Interaction");
            if (interactionUIPrefab == null)
            {
                Debug.LogError("interactionUI 프리팹을 찾을 수 없습니다. 경로를 확인하세요.");
                return;
            }
            else
            {
                Debug.Log("interactionUIPrefab이 정상적으로 로드되었습니다.");
            }
        }

        // 오브젝트에 Collider가 없으면 자동으로 추가
        if (GetComponent<Collider>() == null)
        {
            gameObject.AddComponent<BoxCollider>().isTrigger = true;
            Debug.Log("Collider가 없어서 BoxCollider가 추가되었습니다.");
        }

        interactionUI = Instantiate(interactionUIPrefab, transform.position + uiOffset, Quaternion.identity);
        interactionUI.transform.SetParent(transform, false);
        interactionUI.SetActive(false);
        Debug.Log("interactionUI가 정상적으로 생성되었습니다. 초기 상태는 비활성화입니다.");
    }

    private void Update()
    {
        // 플레이어와 오브젝트 간의 거리 계산
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        // 플레이어와의 상호작용 거리 확인
        if (distanceToPlayer <= 5f)
        {
            if (!ui_Show)
            {
                ui_Show = true;
                Debug.Log("플레이어가 상호작용 범위 내로 들어왔습니다.");
            }
        }
        else
        {
            if (ui_Show)
            {
                ui_Show = false;
                Debug.Log("플레이어가 상호작용 범위를 벗어났습니다.");
            }
        }

        // 상호작용 시간이 경과 중일 때 타이머 갱신
        if (calling && _timeDelta < _workingTime)
        {
            _timeDelta += Time.deltaTime;
        }

        if (_timeDelta >= _workingTime)
        {
            calling = false;
            interactable = true;  // 상호작용 완료 후 다시 상호작용 가능 상태로 변경
        }

        UpdateInteractionUI(); // UI 업데이트
    }

    // UI 상태 업데이트
    void UpdateInteractionUI()
    {
        if (ui_Show)
        {
            if (interactionUI != null && !interactionUI.activeSelf)
            {
                interactionUI.SetActive(true);
                Debug.Log($"UI {interactionUI.name} 활성화됨");
            }

            UpdateUIPosition(Camera.main); // UI 위치 업데이트
        }
        else
        {
            if (interactionUI != null && interactionUI.activeSelf)
            {
                interactionUI.SetActive(false);
                Debug.Log($"UI {interactionUI.name} 비활성화됨");
            }
        }
    }

    public override void Interaction()
    {
        if (!interactable) return;  // 상호작용 가능 상태인지 확인

        base.Interaction();

        interactable = false;  // 상호작용 후 일시적으로 상호작용 불가능 상태로 설정
        calling = true;  // 상호작용 시작
        ui_Show = true;  // UI 표시

        if (Sound != null)
        {
            _audio.PlayOneShot(Sound);
            Debug.Log("사운드가 정상적으로 재생되었습니다.");
        }
        else
        {
            Debug.LogError("사운드가 할당되지 않았습니다.");
        }
    }

    // UI를 오브젝트 위치 근처로 이동
    public override void UpdateUIPosition(Camera camera)
    {
        if (interactionUI != null)
        {
            Vector3 uiPosition = this.transform.position + uiOffset;
            uiPosition.y += Mathf.Sin(Time.time) * 0.1f; // 약간의 흔들림 효과
            interactionUI.transform.position = uiPosition;

            interactionUI.transform.LookAt(camera.transform);
            interactionUI.transform.Rotate(0, 180f, 0); // UI를 카메라 방향으로 회전
        }
    }
}
