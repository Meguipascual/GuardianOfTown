using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerController : Character
{
    public float XLeftBound { get; set; }
    public float XRightBound { get; set; }
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
    
    private KeyCode _rightGateButton;
    private KeyCode _leftGateButton; 
    private KeyCode _accelerateTimeButton;
    private KeyCode _decelerateTimeButton;
    private KeyCode _doubleShootButton;
    private KeyCode _tripleShootButton;
    public bool IsDead { get; set; }
    public int Exp { get; set; }
    public int LevelPoints { get; set; }
    public int CriticalRate { get; set; }
    public int Damage { get; set; }
    public int CriticalDamage { get; set; }
    public int TimeScale { get; set; }

    // Start is called before the first frame update
    void Start()    
    {
        _rightGateButton = ControlButtons._rightGateButton;
        _leftGateButton = ControlButtons._leftGateButton;
        _accelerateTimeButton = ControlButtons._accelerateTimeButton;
        _decelerateTimeButton = ControlButtons._decelerateTimeButton;
        _doubleShootButton = ControlButtons._doubleShootButton;
        _tripleShootButton = ControlButtons._tripleShootButton;
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
        transform.Translate(Vector3.right * Time.deltaTime * Speed * _horizontalInput);

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
        if (TimeScale > 1)
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
            GameManager.Instance._playerLevelPointsText.text = $": {_realTimeLVP}"; 
            GameManager.Instance._playerLevelText.text = $"Lvl: {_realTimeLevel}";
        }
    }
}
