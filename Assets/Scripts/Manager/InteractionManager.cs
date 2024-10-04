using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public Transform player;

    public GameObject interactionUI;
    public GameObject objectToActivate;
    
    public Outline outline;

    public float interactionDistance = 5f;
    private bool inRange = false;

    public KeyCode interactionKey = KeyCode.F;

    public Vector3 offset = new Vector3(0, 1f, 0);
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        interactionUI.SetActive(false);
        outline.enabled = false;
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= interactionDistance)
        {
            if (!inRange)
            {
                inRange = true;
                ShowOutline(true);
                interactionUI.SetActive(true);
            }

            Vector3 screenPosition = mainCamera.WorldToScreenPoint(transform.position + offset);
            interactionUI.transform.position = screenPosition;
            if (Input.GetKeyDown(interactionKey))
            {
                ActivateObject();
            }
        }
        else
        {
            if (inRange)
            {
                inRange = false;
                ShowOutline(false);
                interactionUI.SetActive(false);
            }
        }
    }

    void ShowOutline(bool show)
    {
        if (outline != null)
        {
            outline.enabled = show;
        }
    }
    void ActivateObject()
    {
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(!objectToActivate.activeSelf);
        }
    }
}
