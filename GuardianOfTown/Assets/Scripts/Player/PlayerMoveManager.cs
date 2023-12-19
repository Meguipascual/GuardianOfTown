using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveManager : MonoBehaviour
{
    private Touch _theTouch;
    private Vector2 _touchStartPosition, _touchEndPosition;
    private PlayerController _playerController;
    private float _horizontalInput;


    // Start is called before the first frame update
    void Start()
    {
        _playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
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

        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            TouchMove();
        }
        else
        {
            KeyMove();
        }
    }


    private void TouchMove()
    {
        if (Input.touchCount > 0)
        {
            _theTouch = Input.GetTouch(0);
            if (_theTouch.phase == TouchPhase.Began)
            {
                _touchStartPosition = _theTouch.position;
            }
            else if (_theTouch.phase == TouchPhase.Moved || _theTouch.phase == TouchPhase.Stationary)
            {
                _touchEndPosition = _theTouch.position;
                _horizontalInput = (_touchEndPosition.x - _touchStartPosition.x)/500;

                if (_horizontalInput < -1)
                {
                    _horizontalInput = -1;
                }else if (_horizontalInput > 1)
                {
                    _horizontalInput = 1;
                }
                
                var text = $"H.Input: {_horizontalInput}";
                GameManager.Instance.ChangeAndShowDevText(text);
                if (_horizontalInput != 0)
                {
                    transform.Translate(Vector3.right * Time.deltaTime * _playerController.Speed * _horizontalInput);
                    _playerController.PlayMoveSound();
                }
                else if(_theTouch.phase == TouchPhase.Ended)
                {
                    _touchStartPosition = _theTouch.position;
                    _playerController.moveAudioSource.Stop();
                }
            }
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

    private void KeyMove()
    {
        _horizontalInput = Input.GetAxis("Horizontal");

        var text = $"H.Input: {_horizontalInput}";
        GameManager.Instance.ChangeAndShowDevText(text);

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
