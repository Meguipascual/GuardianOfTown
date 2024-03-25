using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class TouchPlayerMovementManager : MonoBehaviour
{
    [SerializeField] private float _accelerationDelay;
    [SerializeField] private float _currentAcceleration;
    private int _direction;//negative left , positive right
    private PlayerMoveManager _moveManager;
    private PlayerController _playerController;
    private bool _outOfFocus;
    
    // Start is called before the first frame update
    void Start()
    {
        _moveManager = GetComponent<PlayerMoveManager>();
        _playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_outOfFocus) { return; }

        if (_playerController.IsDead || GameManager.Instance.IsGamePaused || GameManager.Instance.IsCountDownActive)
        {
            return;
        }

        foreach (UnityEngine.InputSystem.EnhancedTouch.Touch touch in UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches)
        {
            if (touch.startScreenPosition.x > (Screen.width / 3) * 2 || touch.startScreenPosition.y > (Screen.height / 3)) { continue; }
            if (touch.phase != UnityEngine.InputSystem.TouchPhase.Ended && !double.IsNaN(touch.startScreenPosition.x))
            {
                if (touch.startScreenPosition.x < touch.screenPosition.x)
                {
                    if(_direction < 0){ _currentAcceleration = _accelerationDelay; }
                    _direction = 1;
                    _moveManager.TouchMove(_direction / _currentAcceleration);
                    _currentAcceleration = (_currentAcceleration > 1) ? _currentAcceleration -= Time.deltaTime : 1 ;
                }
                else if (touch.startScreenPosition.x > touch.screenPosition.x)
                {
                    if (_direction > 0) { _currentAcceleration = _accelerationDelay; }
                    _direction = -1;
                    _moveManager.TouchMove(_direction / _currentAcceleration);
                    _currentAcceleration = (_currentAcceleration > 1) ? _currentAcceleration -= Time.deltaTime : 1;
                }
                else
                {
                    _direction = 0;
                    _currentAcceleration = _accelerationDelay;
                    _moveManager.TouchMove(_direction);
                }
            }
            else
            {
                _currentAcceleration = _accelerationDelay;
            }
            return;
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        _outOfFocus = !focus;
        Debug.Log($"Focus State: {focus}");
        if (!focus)
        {
            GameManager.Instance.OpenMenu();
            EnhancedTouchSupport.Disable();
        }
        else
        {
            EnhancedTouchSupport.Enable();
        }
        _direction = 0;
    }

    private void OnApplicationPause(bool pause)
    {
        _outOfFocus = pause;
        if (pause)
        {
            GameManager.Instance.OpenMenu();
            EnhancedTouchSupport.Disable();
        }
        else
        {
            EnhancedTouchSupport.Enable();
        }
        _direction = 0;
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();

    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }
}
