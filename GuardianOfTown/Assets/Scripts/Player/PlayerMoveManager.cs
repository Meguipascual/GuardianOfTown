using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using TouchPhase = UnityEngine.TouchPhase;

public class PlayerMoveManager : MonoBehaviour
{
    private Touch _theTouch;
    private Vector2 _touchStartPosition, _touchEndPosition;
    private CallbackContext _callback;
    private PlayerController _playerController;
    private float _horizontalInput;
    private bool _isTouching;
    [SerializeField] private int _aceleration;//The higher the faster reach max velocity (between 4 and maybe 10 it could work) 


    // Start is called before the first frame update
    void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _isTouching = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_playerController.IsDead || GameManager.Instance.IsGamePaused)
        {
            return;
        }

        for (int i = 0; i < _playerController._animators.Length; i++)
        {
            if (!_playerController._animators[i].name.Equals("Cannon"))
            {
                _playerController._animators[i].SetBool("Right", false);
                _playerController._animators[i].SetBool("Left", false);
            }
        }


        // Check for left and right bounds
        if (transform.position.x < _playerController.XLeftBound)
        {
            transform.position = new Vector3(_playerController.XLeftBound, transform.position.y, transform.position.z);
        }

        if (transform.position.x > _playerController.XRightBound)
        {
            transform.position = new Vector3(_playerController.XRightBound, transform.position.y, transform.position.z);
        }

        if (_callback.ReadValue<Vector2>().x != 0) 
        {
            if (SystemInfo.deviceType == DeviceType.Handheld)
            {
                TouchMove();
            }
            else
            {
                KeyBoardMove(_callback);
            }
        }
        
    }

    public void SelectMovementType(InputAction.CallbackContext context)
    {
        _callback = context;
    }

    private void TouchMove()
    {
        if (Input.touchCount == 0) { return; }

        _isTouching = false;
        if(_theTouch.phase == TouchPhase.Ended || _theTouch.phase == TouchPhase.Canceled)
        {
            _touchStartPosition = new Vector2(Screen.width, Screen.height);
        }

        foreach (var touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began) 
            {
                if (touch.position.x < Screen.width / 2)
                {
                    _touchStartPosition = touch.position;
                    _theTouch = touch;
                    _isTouching = true;
                }    
            }
            else if (touch.position.x < (Screen.width / 3) * 2)
            {
                _theTouch = touch;
                _isTouching = true;
            }
        }
        if (_isTouching)
        {
            TouchMoving();
        }    
    }


    private void TouchMoving()
    {
        //var text = $"H.Input: {context}";
        //GameManager.Instance.ChangeAndShowDevText(text);

        _touchEndPosition = _theTouch.position;
        _horizontalInput = (_touchEndPosition.x - _touchStartPosition.x) / (Screen.width / _aceleration);

        if (_horizontalInput < -1)
        {
            _horizontalInput = -1;
        }
        else if (_horizontalInput > 1)
        {
            _horizontalInput = 1;
        }

        if (_horizontalInput != 0)
        {
            transform.Translate(Vector3.right * Time.deltaTime * _playerController.Speed * _horizontalInput);
            _playerController.PlayMoveSound();
        }
        else
        {
            _playerController.moveAudioSource.Stop();
        }
    
        for (int i = 0; i < _playerController._animators.Length; i++)
        {
            if (!_playerController._animators[i].name.Equals("Cannon"))
            {
                if (_horizontalInput > 0)
                {
                    _playerController._animators[i].SetBool("Right", true);
                }
                else if (_horizontalInput < 0)
                {
                    _playerController._animators[i].SetBool("Left", true);
                }
            }
        }
    }

    private void KeyBoardMove(InputAction.CallbackContext context)
    {
        _horizontalInput = context.ReadValue<Vector2>().x;

        if (_horizontalInput != 0)
        {
            transform.Translate(Vector3.right * Time.deltaTime * _playerController.Speed * _horizontalInput);
            _playerController.PlayMoveSound();
        }
        else
        {
            _playerController.moveAudioSource.Stop();
        }

        for (int i = 0; i < _playerController._animators.Length; i++)
        {
            if (!_playerController._animators[i].name.Equals("Cannon"))
            {
                if (_horizontalInput > 0)
                {
                    _playerController._animators[i].SetBool("Right", true);
                }
                else if (_horizontalInput < 0)
                {
                    _playerController._animators[i].SetBool("Left", true);
                }
            }
        }
    }

}
