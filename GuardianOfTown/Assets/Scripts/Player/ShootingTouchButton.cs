using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using static UnityEngine.InputSystem.InputAction;

public class ShootingTouchButton : MonoBehaviour
{
    public bool isPressed { get; set; }
    private ShootingManager _shootingManager;
    private UnityEngine.InputSystem.EnhancedTouch.Touch _theTouch;
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

            if (touch.startScreenPosition.x < (Screen.width / 3) * 2 || touch.startScreenPosition.y > (Screen.height / 3)) { continue; }
            _theTouch = touch;
            isPressed = true;
        }

        if (isPressed) 
        {
            if(PermanentPowerUpsSettings.Instance.IsContinuousShootActive || PowerUpSettings.Instance.IsContinuousShootInUse) 
            {
                _shootingManager.ShootEasyMode();
            }
            else
            {
                if (_theTouch.phase != UnityEngine.InputSystem.TouchPhase.Began) 
                { 
                    OverHeatedManager.Instance.CoolCannon();
                    return;
                }
                else
                {
                    _shootingManager.TryToShoot();
                    OverHeatedManager.Instance.CoolCannon();
                }
            }
        } 
        else 
        {
            OverHeatedManager.Instance.CoolCannon();
        }
    }
}