using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class TouchPlayerMovementManager : MonoBehaviour
{
    [SerializeField] private float _accelerationDelay;
    [SerializeField] private float _currentAcceleration;
    private int _direction;//negative left , positive right
    private PlayerMoveManager _moveManager;
    
    // Start is called before the first frame update
    void Start()
    {
        _moveManager = GetComponent<PlayerMoveManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (UnityEngine.InputSystem.EnhancedTouch.Touch touch in UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches)
        {
            if (touch.startScreenPosition.x > (Screen.width / 3) * 2 || touch.startScreenPosition.y > (Screen.height / 3) * 2) { continue; }
            if (touch.phase != UnityEngine.InputSystem.TouchPhase.Ended)
            {
                if (touch.startScreenPosition.x < touch.screenPosition.x)
                {
                    if(_direction < 0){ _currentAcceleration = _accelerationDelay; }
                    _direction = 1;
                    _moveManager.TouchMove(1 / _currentAcceleration);
                    _currentAcceleration = (_currentAcceleration > 1) ? _currentAcceleration -= Time.deltaTime : 1 ;
                }
                else if (touch.startScreenPosition.x > touch.screenPosition.x)
                {
                    if (_direction > 0) { _currentAcceleration = _accelerationDelay; }
                    _direction = -1;
                    _moveManager.TouchMove(-1 / _currentAcceleration);
                    _currentAcceleration = (_currentAcceleration > 1) ? _currentAcceleration -= Time.deltaTime : 1;
                }
                else
                {
                    _direction = 0;
                    _currentAcceleration = _accelerationDelay;
                    _moveManager.TouchMove(0);
                }

                if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began || (touch.phase == UnityEngine.InputSystem.TouchPhase.Stationary && touch.phase != UnityEngine.InputSystem.TouchPhase.Moved))
                {

                }
            }
            else
            {
                _currentAcceleration = _accelerationDelay;
            }
        }
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        TouchSimulation.Enable();
        //UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += FingerDown;

    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
        TouchSimulation.Disable();
        //UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown -= FingerDown;
    }

    private void FingerDown(Finger finger)
    {
        if(finger.currentTouch.phase == UnityEngine.InputSystem.TouchPhase.Moved)
        {
            if(finger.currentTouch.startScreenPosition.x < finger.currentTouch.screenPosition.x)
            {
                var text = $"H.Input: {finger}";
                GameManager.Instance.ChangeAndShowDevText(text);
                //1
            }
            else if(finger.currentTouch.startScreenPosition.x > finger.currentTouch.screenPosition.x)
            {
                var text = $"H.Input: {finger}";
                GameManager.Instance.ChangeAndShowDevText(text);
                //-1
            }
        }
    }
}
