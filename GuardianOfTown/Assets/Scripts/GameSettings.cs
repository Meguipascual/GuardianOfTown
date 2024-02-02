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

    public delegate void FrameRateChangeAction();
    public static event FrameRateChangeAction OnChangeFrameRate;

    [SerializeField] private int _frameRate;
    private int _previousFPS;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        _frameRate = 30;
        ChangeFrameRate();  
    }

    private void Update()
    {
        if (_previousFPS != _frameRate) { ChangeFrameRate(); }
    }

    public void ChangeFrameRate()
    {
        Application.targetFrameRate = _previousFPS = _frameRate;
    }
}
