using UnityEngine;
using UnityEngine.UI;

public class ClearInteractiveObj : InteractableObjExtand
{
    [SerializeField] private Transform targetObject; // Clear 대상 오브젝트
    [SerializeField] private GameObject specificObject; // 특정 오브젝트
    [SerializeField] private float clearDistance = 5f;

    private void Start()
    {
        if (!interactable) return;
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

        specificObject.SetActive(true); // 특정 오브젝트 활성화
        SpecitficObject specificObjComponent = specificObject.GetComponent<SpecitficObject>();
    }
}
