using UnityEngine;
using UnityEngine.UI;

public class ClearInteractiveObj : InteractableObjExtand
{
    [SerializeField] private Transform targetObject; // Clear ��� ������Ʈ
    [SerializeField] private GameObject specificObject; // Ư�� ������Ʈ
    [SerializeField] private float clearDistance = 5f;

    private void Start()
    {
        if (!interactable) return;
    }

    public override void Interaction()
    {
        if (!interactable) return;

        // Ư�� ������Ʈ Ȱ��ȭ
        MissionObjIndicator missionObjIndicator = FindObjectOfType<MissionObjIndicator>();
        if (missionObjIndicator != null)
        {
            missionObjIndicator.HideUI(); // UI ��Ȱ��ȭ
        }

        specificObject.SetActive(true); // Ư�� ������Ʈ Ȱ��ȭ
        SpecitficObject specificObjComponent = specificObject.GetComponent<SpecitficObject>();
    }
}
