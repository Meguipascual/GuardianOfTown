using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerController : Character
{
    [SerializeField] private AudioSource _moveAudioSource;
    [SerializeField] private AudioSource _leveUpAudioSource;
    [SerializeField] private AudioSource _powerUpAudioSource;
    [SerializeField] private AudioSource _damageReceivedSource;
    [SerializeField] private float _pitch;
    private float _horizontalInput;
    private int _realTimeLevel;
    private int _realTimeLVP;
    private int _realTimeEXP;
    private FillHealthBar _fillHealthBar;
    private ChangeGateManager _changeGateManager;
    private ShootingManager _shootingManager;
    private PermanentPowerUpsSettings _permanentPowerUpsSettings;
    private Animator [] _animators;
    public ParticleSystem shieldParticleSystem;
    public Image wiiImage;
    public Image yeiiImage;
    public Image ouchImage;
    private KeyCode _rightGateButton;
    private KeyCode _leftGateButton; 
    private KeyCode _accelerateTimeButton;
    private KeyCode _decelerateTimeButton;
    private KeyCode _doubleShootButton;
    private KeyCode _tripleShootButton;
    private KeyCode _shoot;
    private KeyCode _alternateShoot;
    public bool IsDead { get; set; }
    public int Exp { get; set; }
    public int LevelPoints { get; set; }
    public int CriticalRate { get; set; }
    public int Damage { get; set; }
    public int CriticalDamage { get; set; }
    public float TimeScale { get; set; }
    public float XLeftBound { get; set; }
    public float XRightBound { get; set; }

    // Start is called before the first frame update
    void Start()    
    {
        _rightGateButton = ControlButtons._rightGateButton;
        _leftGateButton = ControlButtons._leftGateButton;
        _accelerateTimeButton = ControlButtons._accelerateTimeButton;
        _decelerateTimeButton = ControlButtons._decelerateTimeButton;
        _doubleShootButton = ControlButtons._doubleShootButton;
        _tripleShootButton = ControlButtons._tripleShootButton;
        _alternateShoot = ControlButtons._alterShoot;
        _shoot = ControlButtons._shoot;
        XLeftBound = DataPersistantManager.Instance.SpawnBoundariesLeft[0];
        XRightBound = DataPersistantManager.Instance.SpawnBoundariesRight[0];
        shieldParticleSystem = GetComponentInChildren<ParticleSystem>();
        _shootingManager = GetComponentInChildren<ShootingManager>();
        _fillHealthBar = FindObjectOfType<FillHealthBar>();
        _changeGateManager = FindObjectOfType<ChangeGateManager>();
        DataPersistantManager.Instance.LoadPlayerStats();
        _animators = GetComponentsInChildren<Animator>();
        _permanentPowerUpsSettings = PermanentPowerUpsSettings.Instance;

        if (_permanentPowerUpsSettings.IsFrontSwordActive)
        {
            _permanentPowerUpsSettings.ActivateSword();
        }
        if (_permanentPowerUpsSettings.IsBackShootActive)
        {
            _permanentPowerUpsSettings.ActivateBackCannon();
        }
        _realTimeLevel = Level;
        _realTimeLVP = LevelPoints;
        _realTimeEXP = Exp;
        _leveUpAudioSource.volume = .5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDead || GameManager.Instance.IsGamePaused)
        {
            return;
        }

        for (int i = 0; i < _animators.Length; i++)
        {
            if (!_animators[i].name.Equals("Cannon"))
            {
                _animators[i].SetBool("Right", false);
                _animators[i].SetBool("Left", false);
            }
        }

        TryToMove(); 

        if (Input.GetKeyDown(_rightGateButton))
        {
            _changeGateManager.RightButtonClicked();
        }
        if (Input.GetKeyDown(_leftGateButton))
        {
            _changeGateManager.LeftButtonClicked();
        }
        if (Input.GetKeyDown(_accelerateTimeButton))
        {
            AccelerateTime();
        }
        if (Input.GetKeyDown(_decelerateTimeButton))
        {
            DecelerateTime();
        }
        if (Input.GetKeyDown(_doubleShootButton))
        {
            _permanentPowerUpsSettings.ActivateDoubleShoot();
            var text = $"Double Shoot Activated";
            GameManager.Instance.ChangeAndShowDevText(text);
        }
        if (Input.GetKeyDown(_tripleShootButton))
        {
            _permanentPowerUpsSettings.ActivateTripleShoot();
            var text = $"Triple Shoot Activated";
            GameManager.Instance.ChangeAndShowDevText(text);
        }
        if (GameSettings.Instance.IsEasyModeActive || PermanentPowerUpsSettings.Instance.IsInfiniteContinuousShootActive || PowerUpSettings.Instance.IsContinuousShootInUse)
        {
            _shootingManager.ShootEasyMode();
            return;
        }
        if (Input.GetKeyDown(_shoot) || Input.GetKeyDown(_alternateShoot))
        {
            _shootingManager.TryToShoot();
        }
    }


    public override void Die()
    {
        Debug.Log("GameOver");
        IsDead = true;
        //GameOver
    }

    public override void TryToMove()
    {
        // Check for left and right bounds
        if (transform.position.x < XLeftBound)
        {
            transform.position = new Vector3(XLeftBound, transform.position.y, transform.position.z);
        }

        if (transform.position.x > XRightBound)
        {
            transform.position = new Vector3(XRightBound, transform.position.y, transform.position.z);
        }

        // Player movement left to right
        _horizontalInput = Input.GetAxis("Horizontal");

        if (_horizontalInput != 0)
        {
            transform.Translate(Vector3.right * Time.deltaTime * Speed * _horizontalInput);
            PlayMoveSound();
        }
        else
        {
            _moveAudioSource.Stop();
        }

        for (int i = 0; i < _animators.Length; i++)
        {
            if (!_animators[i].name.Equals("Cannon"))
            {
                if(_horizontalInput > 0)
                {
                    _animators[i].SetBool("Right", true); 
                }
                else if (_horizontalInput < 0)
                {
                    _animators[i].SetBool("Left", true);
                }
            }
        }
    }

    public void ComprobateLifeRemaining ()
    {
        _fillHealthBar.FillSliderValue();
        if (HP <= 0)
        {
            Die();
        }
    }

    public void TownReceiveDamage()
    {
        var shields = GameManager.Instance._townHpText.GetComponentsInChildren(
            GameManager.Instance.TownHpShields[GameManager.Instance.TownHpShields.Count - 1].GetType()
            );

        GameManager.Instance.TownHpShieldsDamaged++;
        shields[shields.Length - GameManager.Instance.TownHpShieldsDamaged].GetComponent<Image>().sprite 
            = GameManager.Instance.TownDamagedShieldImages[0].GetComponent<Image>().sprite;

        if(GameManager.Instance.TownHpShields.Count == GameManager.Instance.TownHpShieldsDamaged)
        {
            Die();
        }
    }

    
    public bool IsCritical()
    {
        var isCritical = false;
        var random = Random.Range(0, 100);
        float criticalDamageFloat = (float)CriticalDamage / 100;
        if (CriticalRate > random)
        {
            isCritical = true;
            Damage = (int)(Attack * (criticalDamageFloat));
        }
        else
        {
            Damage = Attack;
        }
        return isCritical;
    }

    public void FillSliderValue()
    {
        _fillHealthBar.FillSliderValue();
    }

    public void AccelerateTime()
    {
        if (TimeScale < 20)
        {
            TimeScale *= 2;
            Time.timeScale = TimeScale;
        }
        var text = $"TimeScale x{TimeScale}";
        GameManager.Instance.ChangeAndShowDevText(text);
    }

    public void DecelerateTime()
    {
        if (TimeScale > .25f)
        {
            TimeScale /= 2;
            Time.timeScale = TimeScale;
        }
        var text = $"TimeScale x{TimeScale}";
        GameManager.Instance.ChangeAndShowDevText(text);
    }

    public void RealTimeLevelUp(int exp)
    {
        _realTimeEXP += exp;

        if (_realTimeEXP > 20 * _realTimeLevel)
        {
            _realTimeEXP -= 20 * _realTimeLevel;
            _realTimeLVP += 2;
            _realTimeLevel++;
            PlayLevelUpSound();
            StartCoroutine(GameManager.Instance.ShowLevelUpText());
            GameManager.Instance._playerLevelPointsText.text = $": {_realTimeLVP}"; 
            GameManager.Instance._playerLevelText.text = $"Lvl: {_realTimeLevel}";
        }
    }

    

    public void PlayMoveSound()
    {
        if (!_moveAudioSource.isPlaying)
        {
            _moveAudioSource.pitch = _pitch;
            _moveAudioSource.Play();
        }
    }

    public void PlayLevelUpSound()
    {
        _leveUpAudioSource.Play();
    }

    public void PlayDamageReceivedSound()
    {
        _damageReceivedSource.Play();
    }

    public void PlayPowerUpSound()
    {
        _powerUpAudioSource.pitch = .9f;
        _powerUpAudioSource.Play();
    }

    public void ShowWiiImageInSeconds(float seconds)
    {
        StartCoroutine(ShowWiiImage(seconds));
    }

    public void ShowYeiiImageInSeconds(float seconds)
    {
        StartCoroutine(ShowYeiiImage(seconds));
    }

    public void ShowOuchImageInSeconds(float seconds)
    {
        StartCoroutine(ShowOuchImage(seconds));
    }

    private IEnumerator ShowWiiImage(float seconds)
    {
        wiiImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(seconds);
        wiiImage.gameObject.SetActive(false);
    }

    private IEnumerator ShowYeiiImage(float seconds)
    {
        yeiiImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(seconds);
        yeiiImage.gameObject.SetActive(false);
    }

    private IEnumerator ShowOuchImage(float seconds)
    {
        ouchImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(seconds);
        ouchImage.gameObject.SetActive(false);
    }
}
