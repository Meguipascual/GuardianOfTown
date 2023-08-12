using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsButtonManager : MonoBehaviour
{

    public void ActivateFront()
    {
        GameSettings.Instance.IsTopViewModeActive = false;
        GameSettings.Instance.IsFrontViewModeActive = true;
    }

    public void ActivateTop()
    {
        GameSettings.Instance.IsTopViewModeActive = true;
        GameSettings.Instance.IsFrontViewModeActive = false;
    }

    public void ToggleEasyMode()
    {
        GameSettings.Instance.IsEasyModeActive = !GameSettings.Instance.IsEasyModeActive;
        GameSettings.Instance.IsDeveloperModeActive = false;
    }
    public void ToggleDeveloperMode()
    {
        GameSettings.Instance.IsDeveloperModeActive = !GameSettings.Instance.IsDeveloperModeActive;
        GameSettings.Instance.IsEasyModeActive = false;
    }
    public void ActivateNormalMode()
    {
        GameSettings.Instance.IsEasyModeActive = false;
        GameSettings.Instance.IsDeveloperModeActive = false;
    }
}
