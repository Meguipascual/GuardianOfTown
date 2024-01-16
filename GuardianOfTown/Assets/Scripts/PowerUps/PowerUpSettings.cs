using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PowerUpSettings : MonoBehaviour
{
    public static PowerUpSettings Instance;
    private PlayerController _playerController;
    private GameObject _bulletSliderGameobject;
    private Slider _bulletSlider;
    private GameObject _speedSliderGameobject;
    private Slider _speedSlider;
    public bool IsContinuousShootInUse {  get; set; }
    public bool IsSpeedIncreased {  get; set; }
    public float SpeedAmount {  get; set; }
    public float SpeedTimerMax { get; set; }
    public float BulletTimerMax {  get; set; }
    public float PreviousPlayerSpeed {  get; set; }

    private float _speedTimer;
    private float _bulletTimer;
    [SerializeField] 

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if (IsContinuousShootInUse) 
        {
            if(_bulletTimer > 0)
            {
                _bulletTimer -= Time.deltaTime;
                _bulletSlider.value = _bulletTimer / BulletTimerMax;
            }
            else
            {
                DeactivateBulletTimer();
            }
        }
        

        if (IsSpeedIncreased)
        {
            if (_speedTimer > 0)
            {
                _speedTimer -= Time.deltaTime;
                _speedSlider.value = _speedTimer / SpeedTimerMax;
            }
            else
            {
                DeactivateSpeedTimer();
            }
        }
    }

    public void ActivateBulletTimer(float timerLimit)
    {
        if (IsContinuousShootInUse) { Debug.Log("Already in use");_bulletTimer = timerLimit; return; }
        IsContinuousShootInUse = true;
        _bulletTimer = timerLimit;
        BulletTimerMax = timerLimit;
        _bulletSliderGameobject = PowerUpTimerSliderManager.Instance.InstantiatePowerUpSliderTimer(0);
        _bulletSlider = _bulletSliderGameobject.GetComponentInChildren<Slider>();
        Debug.Log($"deactivate easy mode in {timerLimit} seconds");
    }

    private void DeactivateBulletTimer()
    {
        Debug.Log($"deactivate easy mode");
        _bulletTimer = 0;
        BulletTimerMax = 0;
        IsContinuousShootInUse = false;
        PowerUpTimerSliderManager.Instance.RemoveSlider(_bulletSliderGameobject);
        Destroy(_bulletSliderGameobject);
        Destroy(_bulletSlider);
    }

    private void DeactivateSpeedTimer()
    {
        IsSpeedIncreased = false;
        _playerController.Speed = PreviousPlayerSpeed;
        SpeedAmount = 0;
        _speedTimer = 0;
        SpeedTimerMax = 0;
        PowerUpTimerSliderManager.Instance.RemoveSlider(_speedSliderGameobject);
        GameManager.Instance._menuPlayerSpeedText.text = $"Speed: {_playerController.Speed}";
        Debug.Log("Speed Reverted");
        Destroy(_speedSlider);
        Destroy(_speedSliderGameobject);
    }

    public void ActivateSpeedTimer(float amount, float timerLimit)
    {
        if (IsSpeedIncreased) 
        {
            if (amount == SpeedAmount)
            {
                _speedTimer = timerLimit;
            }
            return; 
        }

        IsSpeedIncreased = true;
        SpeedAmount = amount;
        SpeedTimerMax = timerLimit;
        _speedTimer = timerLimit;
        PreviousPlayerSpeed = _playerController.Speed;
        _playerController.Speed += SpeedAmount;
        Debug.Log("Speed Augmented");
        GameManager.Instance._menuPlayerSpeedText.text = $"Speed: {_playerController.Speed}";
        _speedSliderGameobject = PowerUpTimerSliderManager.Instance.InstantiatePowerUpSliderTimer(1);
        _speedSlider = _speedSliderGameobject.GetComponentInChildren<Slider>();
    }

}
