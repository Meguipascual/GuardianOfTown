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
    private FillHealthBar _fillHealthBar;
    private ChangeGateManager _changeGateManager;
    private ShootingManager _shootingManager;
    private PermanentPowerUpsSettings _permanentPowerUpsSettings;
    public ParticleSystem shieldParticleSystem;
    
    private KeyCode _rightGateButton;
    private KeyCode _leftGateButton; 
    private KeyCode _accelerateTimeButton;
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
        XLeftBound = DataPersistantManager.Instance.SpawnBoundariesLeft[0];
        XRightBound = DataPersistantManager.Instance.SpawnBoundariesRight[0];
        shieldParticleSystem = GetComponentInChildren<ParticleSystem>();
        _shootingManager = GetComponentInChildren<ShootingManager>();
        _fillHealthBar = FindObjectOfType<FillHealthBar>();
        _changeGateManager = FindObjectOfType<ChangeGateManager>();
        DataPersistantManager.Instance.LoadPlayerStats();
        _permanentPowerUpsSettings = PermanentPowerUpsSettings.Instance;

        if (_permanentPowerUpsSettings.IsFrontSwordActive)
        {
            _permanentPowerUpsSettings.ActivateSword();
        }
        if (_permanentPowerUpsSettings.IsBackShootActive)
        {
            _permanentPowerUpsSettings.ActivateBackCannon();
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (IsDead || GameManager.SharedInstance.IsGamePaused)
        {
            return;
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
        var shields = GameManager.SharedInstance._townHpText.GetComponentsInChildren(
            GameManager.SharedInstance.TownHpShields[GameManager.SharedInstance.TownHpShields.Count - 1].GetType()
            );

        GameManager.SharedInstance.TownHpShieldsDamaged++;
        shields[shields.Length - GameManager.SharedInstance.TownHpShieldsDamaged].GetComponent<Image>().sprite 
            = GameManager.SharedInstance.TownDamagedShieldImages[0].GetComponent<Image>().sprite;

        if(GameManager.SharedInstance.TownHpShields.Count == GameManager.SharedInstance.TownHpShieldsDamaged)
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
        else
        {
            Time.timeScale = 1;
            TimeScale = 1;
        }
        var text = $"TimeScale x{TimeScale}";
        GameManager.SharedInstance.ChangeAndShowDevText(text);
    }
}
