using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingsButtonManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _optionChangedAlertText;
    [SerializeField] private int _messageDuration = 2;
    public void ActivateFront()
    {
        GameSettings.Instance.IsTopViewModeActive = false;
        GameSettings.Instance.IsFrontViewModeActive = true;
        StopAllCoroutines();
        StartCoroutine(ShowOptionChangeMessage($"Front viewpoint activated"));
    }

    public void ActivateTop()
    {
        GameSettings.Instance.IsTopViewModeActive = true;
        GameSettings.Instance.IsFrontViewModeActive = false;
        StopAllCoroutines();
        StartCoroutine(ShowOptionChangeMessage($"Top viewpoint activated"));
    }

    public void ToggleEasyMode()
    {
        GameSettings.Instance.IsEasyModeActive = !GameSettings.Instance.IsEasyModeActive;
        GameSettings.Instance.IsDeveloperModeActive = false;
        StopAllCoroutines();
        StartCoroutine(ShowOptionChangeMessage($"Easy Gamemode activated" +
            $"(not developed yet)"));
    }
    public void ToggleDeveloperMode()
    {
        GameSettings.Instance.IsDeveloperModeActive = !GameSettings.Instance.IsDeveloperModeActive;
        GameSettings.Instance.IsEasyModeActive = false;
        StopAllCoroutines();
        StartCoroutine(ShowOptionChangeMessage($"Dev Gamemode activated" +
            $"(not developed yet)"));
    }
    public void ActivateNormalMode()
    {
        GameSettings.Instance.IsEasyModeActive = false;
        GameSettings.Instance.IsDeveloperModeActive = false;
        StopAllCoroutines();
        StartCoroutine(ShowOptionChangeMessage($"Normal Gamemode activated" +
            $"(the only Gamemode)"));
    }

    IEnumerator ShowOptionChangeMessage(string message)
    {
        _optionChangedAlertText.text = message;
        _optionChangedAlertText.gameObject.SetActive(true);
        yield return new WaitForSeconds (_messageDuration);
        _optionChangedAlertText.gameObject.SetActive(false);
    }
}
