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
    [SerializeField] float _workingTime = 8f; // ��ȣ�ۿ��� �Ϸ�Ǳ������ �ð�
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
            Debug.Log("AudioSource�� ��� �ڵ����� �߰��Ǿ����ϴ�.");
        }

        player = GameObject.FindWithTag("Player");

        if (Sound == null)
        {
            Sound = Resources.Load<AudioClip>("ElevatorError");
            if (Sound == null)
            {
                Debug.LogError("Sound ������ ã�� �� �����ϴ�. ��θ� Ȯ���ϼ���.");
            }
            else
            {
                Debug.Log("Sound ������ ���������� �ε�Ǿ����ϴ�.");
            }
        }

        if (interactionUIPrefab == null)
        {
            interactionUIPrefab = Resources.Load<GameObject>("Prefabs/UI/Object Interaction");
            if (interactionUIPrefab == null)
            {
                Debug.LogError("interactionUI �������� ã�� �� �����ϴ�. ��θ� Ȯ���ϼ���.");
                return;
            }
            else
            {
                Debug.Log("interactionUIPrefab�� ���������� �ε�Ǿ����ϴ�.");
            }
        }

        // ������Ʈ�� Collider�� ������ �ڵ����� �߰�
        if (GetComponent<Collider>() == null)
        {
            gameObject.AddComponent<BoxCollider>().isTrigger = true;
            Debug.Log("Collider�� ��� BoxCollider�� �߰��Ǿ����ϴ�.");
        }

        interactionUI = Instantiate(interactionUIPrefab, transform.position + uiOffset, Quaternion.identity);
        interactionUI.transform.SetParent(transform, false);
        interactionUI.SetActive(false);
        Debug.Log("interactionUI�� ���������� �����Ǿ����ϴ�. �ʱ� ���´� ��Ȱ��ȭ�Դϴ�.");
    }

    private void Update()
    {
        // �÷��̾�� ������Ʈ ���� �Ÿ� ���
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        // �÷��̾���� ��ȣ�ۿ� �Ÿ� Ȯ��
        if (distanceToPlayer <= 5f)
        {
            if (!ui_Show)
            {
                ui_Show = true;
                Debug.Log("�÷��̾ ��ȣ�ۿ� ���� ���� ���Խ��ϴ�.");
            }
        }
        else
        {
            if (ui_Show)
            {
                ui_Show = false;
                Debug.Log("�÷��̾ ��ȣ�ۿ� ������ ������ϴ�.");
            }
        }

        // ��ȣ�ۿ� �ð��� ��� ���� �� Ÿ�̸� ����
        if (calling && _timeDelta < _workingTime)
        {
            _timeDelta += Time.deltaTime;
        }

        if (_timeDelta >= _workingTime)
        {
            calling = false;
            interactable = true;  // ��ȣ�ۿ� �Ϸ� �� �ٽ� ��ȣ�ۿ� ���� ���·� ����
        }

        UpdateInteractionUI(); // UI ������Ʈ
    }

    // UI ���� ������Ʈ
    void UpdateInteractionUI()
    {
        if (ui_Show)
        {
            if (interactionUI != null && !interactionUI.activeSelf)
            {
                interactionUI.SetActive(true);
                Debug.Log($"UI {interactionUI.name} Ȱ��ȭ��");
            }

            UpdateUIPosition(Camera.main); // UI ��ġ ������Ʈ
        }
        else
        {
            if (interactionUI != null && interactionUI.activeSelf)
            {
                interactionUI.SetActive(false);
                Debug.Log($"UI {interactionUI.name} ��Ȱ��ȭ��");
            }
        }
    }

    public override void Interaction()
    {
        if (!interactable) return;  // ��ȣ�ۿ� ���� �������� Ȯ��

        base.Interaction();

        interactable = false;  // ��ȣ�ۿ� �� �Ͻ������� ��ȣ�ۿ� �Ұ��� ���·� ����
        calling = true;  // ��ȣ�ۿ� ����
        ui_Show = true;  // UI ǥ��

        if (Sound != null)
        {
            _audio.PlayOneShot(Sound);
            Debug.Log("���尡 ���������� ����Ǿ����ϴ�.");
        }
        else
        {
            Debug.LogError("���尡 �Ҵ���� �ʾҽ��ϴ�.");
        }
    }

    // UI�� ������Ʈ ��ġ ��ó�� �̵�
    public override void UpdateUIPosition(Camera camera)
    {
        if (interactionUI != null)
        {
            Vector3 uiPosition = this.transform.position + uiOffset;
            uiPosition.y += Mathf.Sin(Time.time) * 0.1f; // �ణ�� ��鸲 ȿ��
            interactionUI.transform.position = uiPosition;

            interactionUI.transform.LookAt(camera.transform);
            interactionUI.transform.Rotate(0, 180f, 0); // UI�� ī�޶� �������� ȸ��
        }
    }
}
