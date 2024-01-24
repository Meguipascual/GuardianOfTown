using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class TouchPlayerMovementManager : MonoBehaviour
{
    private PlayerMoveManager _moveManager;

    // Start is called before the first frame update
    void Start()
    {
        _moveManager = GetComponent<PlayerMoveManager>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (UnityEngine.InputSystem.EnhancedTouch.Touch touch in UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches)
        {
            if (touch.phase != UnityEngine.InputSystem.TouchPhase.Ended)
            {
                if (touch.startScreenPosition.x < touch.screenPosition.x)
                {
                    _moveManager.TouchMove(1);
                    //1
                }
                else if (touch.startScreenPosition.x > touch.screenPosition.x)
                {
                    _moveManager.TouchMove(-1);
                    //-1
                }
                else
                {
                    _moveManager.TouchMove(0);
                    //0
                }

                if(touch.phase == UnityEngine.InputSystem.TouchPhase.Began || (touch.phase == UnityEngine.InputSystem.TouchPhase.Stationary && touch.phase != UnityEngine.InputSystem.TouchPhase.Moved))
                {

                }
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
