using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance;
    public bool IsEasyModeActive { get; set; } //easy mode with autoshooting
    public bool IsDeveloperModeActive { get; set; } //activate buttons that can create any enemy, powerup or wave
    public bool IsFrontViewModeActive { get; set; } //activate the 3 person perspective camera and pannels
    public bool IsTopViewModeActive { get; set; } //activate top view


    private void ComprobateInstance()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Awake()
    {
        ComprobateInstance();
        DontDestroyOnLoad(gameObject);
    }

    public void ActivateFront()
    {
        IsTopViewModeActive = false;
        IsFrontViewModeActive = true;
    }

    public void ActivateTop()
    {
        IsTopViewModeActive = true;
        IsFrontViewModeActive = false;
    }
    
    public void ToggleEasyMode()
    {
        IsEasyModeActive = !IsEasyModeActive;
    }
    public void ToggleDeveloperMode()
    {
        IsDeveloperModeActive = !IsDeveloperModeActive;
    }
}
