using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootingTouchButton : MonoBehaviour
{
    public bool isPressed;
    private ShootingManager _shootingManager;
    private UnityEngine.InputSystem.EnhancedTouch.Touch _theTouch;
    private Vector2 _touchStartPosition, _touchEndPosition;
    private PlayerController _playerController;

    private void Start()
    {
        _shootingManager = FindObjectOfType<ShootingManager>();
        _playerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if (SystemInfo.deviceType != DeviceType.Handheld) { return; }

        if (_playerController.IsDead || GameManager.Instance.IsGamePaused) { return; }

        if (UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches.Count <= 0) { OverHeatedManager.Instance.CoolCannon(); return; }

        isPressed = false;

        foreach (UnityEngine.InputSystem.EnhancedTouch.Touch touch in UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches)
        {
            if (touch.screenPosition.x > (Screen.width / 3) * 2 && touch.screenPosition.y < Screen.height / 3)
            {
                _theTouch = touch;
                isPressed = true;
            }
        }

        if (isPressed)
        {
            _shootingManager.ShootEasyMode();
        }
        else
        {
            OverHeatedManager.Instance.CoolCannon();
        }
    }
}