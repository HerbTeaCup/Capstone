using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour, IManager
{
    public static UIManager Instance;

    // Detection UI
    [SerializeField] GameObject weakDetectionUI; // '?' player found
    [SerializeField] GameObject strongDetectionUI; // '!' player completely found

    // Interaction UI
    [SerializeField] GameObject interactionUI;
    [SerializeField] GameObject stealthUI;

    public System.Action UpdateDelegate = null;
    public System.Action LateDelegate = null;

    public void Updater() => UpdateDelegate?.Invoke();
    public void LateUpdater() => LateDelegate?.Invoke();

    public void Clear()
    {
        UpdateDelegate = null;
        LateDelegate = null;
    }

    // Detection UI ������Ʈ
    public void UpdateDetectionUI(bool showWeakDetectionUI, bool showStrongDetectionUI)
    {
        weakDetectionUI.SetActive(showWeakDetectionUI);
        strongDetectionUI.SetActive(showStrongDetectionUI);
    }

    // Interaction UI ������Ʈ
    public void ShowInteractionUI(bool show)
    {
        interactionUI.SetActive(show);
    }

    // Stealth UI ������Ʈ
    public void ShowStealthUI(bool show)
    {
        stealthUI.SetActive(show);
    }
}
