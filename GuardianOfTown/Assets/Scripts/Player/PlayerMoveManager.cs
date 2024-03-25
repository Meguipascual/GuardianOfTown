using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerMoveManager : MonoBehaviour
{
    private CallbackContext _moveCallback;
    private CallbackContext _brakeCallback;
    private PlayerController _playerController;
    private float _horizontalInput;
    public float CurrentSpeed { get; set; }
    [SerializeField] private float _slowMovementSpeed;
    public bool IsPlayerBrakeOn {  get; set; }

    // Start is called before the first frame update
    void Start()
    {
        _playerController = GetComponent<PlayerController>();
        CurrentSpeed = DataPersistantManager.Instance.SavedPlayerSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (_playerController.IsDead || GameManager.Instance.IsGamePaused || GameManager.Instance.IsCountDownActive)
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

        if (_brakeCallback.phase == InputActionPhase.Started || _brakeCallback.phase == InputActionPhase.Performed)
        {
            if (IsPlayerBrakeOn)
            { 
                _playerController.Speed = _slowMovementSpeed; 
            } 
            else
            {
                CurrentSpeed = _playerController.Speed;
                _playerController.Speed = _slowMovementSpeed;
                IsPlayerBrakeOn = true;
            }
            
        }
        else
        {
            _playerController.Speed = CurrentSpeed;
            IsPlayerBrakeOn = false;
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
        if (_moveCallback.ReadValue<Vector2>().x != 0)
        {
            KeyBoardMove(_moveCallback);
        }
        else
        {
            _playerController.moveAudioSource.Stop();
        }
    }

    public void SelectMovementType(InputAction.CallbackContext context)
    {
        _moveCallback = context;
    }

    public void BrakeMovement(InputAction.CallbackContext context)
    {
        _brakeCallback = context;
    }

    private void KeyBoardMove(InputAction.CallbackContext context)
    {
        _horizontalInput = context.ReadValue<Vector2>().x;
        //var text = $"H.Input: {context.ReadValue<Vector2>()}";
        //GameManager.Instance.ChangeAndShowDevText(text);
        transform.Translate(Vector3.right * Time.deltaTime * _playerController.Speed * _horizontalInput);
        _playerController.PlayMoveSound();

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
    public void TouchMove(float horizontalInput)
    {
        
        var text = $"H.Input: {horizontalInput}";
        GameManager.Instance.ChangeAndShowDevText(text);

        if(horizontalInput < -1 || horizontalInput > 1) { return;}

        transform.Translate(Vector3.right * Time.deltaTime * _playerController.Speed * horizontalInput);
        _playerController.PlayMoveSound();

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
