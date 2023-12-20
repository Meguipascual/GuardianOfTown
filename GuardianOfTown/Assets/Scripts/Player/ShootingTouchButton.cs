using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootingTouchButton : MonoBehaviour
{
    public bool isPressed;
    private ShootingManager _shootingManager;
    private Touch _theTouch;
    private Vector2 _touchStartPosition, _touchEndPosition;
    private PlayerController _playerController;

    private void Start()
    {
        _shootingManager = FindObjectOfType<ShootingManager>();
        _playerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if (_playerController.IsDead || GameManager.Instance.IsGamePaused)
        {
            return;
        }
        if (Input.touchCount == 0) { return; }

        isPressed = false;

        foreach (var touch in Input.touches)
        {
            if (touch.position.x > (Screen.width / 3) * 2 && touch.position.y < Screen.height/3) 
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