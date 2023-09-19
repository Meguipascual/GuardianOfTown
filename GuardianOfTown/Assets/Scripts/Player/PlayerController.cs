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
    public ParticleSystem shieldParticleSystem;
    [SerializeField]private Vector3 offset = new Vector3(0, 0, 1);
    [SerializeField] private float _bulletTimer;//Timer to know when to shoot again
    [SerializeField] private float _bulletDelay;//Time between bullets in continuous shooting
    private KeyCode _shoot;
    private KeyCode _alternateShoot;
    private KeyCode _rightGateButton;
    private KeyCode _leftGateButton;

    public bool IsDead { get; set; }
    public int Exp { get; set; }
    public int LevelPoints { get; set; }
    public int CriticalRate { get; set; }
    public int Damage { get; set; }
    public float CriticalDamage { get; set; }

    // Start is called before the first frame update
    void Start()    
    {
        _bulletTimer = 0;
        _bulletDelay = 0.2f;
        _shoot = ControlButtons._shoot;
        _alternateShoot = ControlButtons._alterShoot;
        _rightGateButton = ControlButtons._rightGateButton;
        _leftGateButton = ControlButtons._leftGateButton;
        XLeftBound = DataPersistantManager.Instance.SpawnBoundariesLeft[0];
        XRightBound = DataPersistantManager.Instance.SpawnBoundariesRight[0];
        shieldParticleSystem = GetComponentInChildren<ParticleSystem>();
        _fillHealthBar = FindObjectOfType<FillHealthBar>();
        _changeGateManager = FindObjectOfType<ChangeGateManager>();
        DataPersistantManager.Instance.LoadPlayerStats();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDead || GameManager.SharedInstance.IsGamePaused)
        {
            return;
        }

        TryToMove();

        if (GameSettings.Instance.IsEasyModeActive)
        {
            ShootEasyMode();
        }
        else
        {
            if(OverHeatedManager.Instance._cannonOverHeatedTimer > 0)
            {
                OverHeatedManager.Instance.CoolCannon();
            }

            if (Input.GetKeyDown(_shoot) || Input.GetKeyDown(_alternateShoot))
            {
                Shoot();
            }
        }

        if (Input.GetKeyDown(_rightGateButton))
        {
            _changeGateManager.RightButtonClicked();
        }
        if (Input.GetKeyDown(_leftGateButton))
        {
            _changeGateManager.LeftButtonClicked();
        }
    }

    private void ShootEasyMode()
    {
        if (OverHeatedManager.Instance.IsOverheatedCannon())
        {
            OverHeatedManager.Instance.CoolCannon();
            return;
        }
        if (Input.GetKey(_shoot) || Input.GetKey(_alternateShoot))
        {
            OverHeatedManager.Instance.HeatCannon();
            _bulletTimer += Time.deltaTime;
            if(_bulletTimer >= _bulletDelay)
            {
                Shoot();
                _bulletTimer = 0;
            }
        }
        else
        {
            OverHeatedManager.Instance.CoolCannon();
        }
    }

    private void Shoot()
    {
        // Get an object object from the pool
        GameObject pooledProjectile = ObjectPooler.SharedInstance.GetPooledObject();
        if (pooledProjectile != null)
        {
            pooledProjectile.SetActive(true); // activate it
            pooledProjectile.transform.position = transform.position + offset; // position it at player
            ObjectPooler.ProjectileCount--;
            GameManager.SharedInstance._projectileText.text = "" + ObjectPooler.ProjectileCount;
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
        shields[shields.Length-1].gameObject.SetActive(false);
        GameManager.SharedInstance.TownHpShields[GameManager.SharedInstance.TownHpShields.Count-1].gameObject.SetActive(false);
        GameManager.SharedInstance.TownHpShields.RemoveAt(GameManager.SharedInstance.TownHpShields.Count - 1);
        if(GameManager.SharedInstance.TownHpShields.Count <= 0)
        {
            Die();
        }
    }

    public override void LevelUp() 
    {
        int randomUpgrade;
        if (Exp > 10*Level)
        {
            LevelPoints += 2;
            HP = HpMax;
            //GameManager.SharedInstance.playerLevelPointsText.text = $"LP: {LevelPoints}";

            //This part of the code are going to change to a method that openned when player will be going to levelup
            for (int i = 0; i < LevelPoints; i++)
            {
                HP += 5;
                if(CriticalRate >= 100)
                {
                    randomUpgrade = Random.Range(0, 3);
                }
                else
                {
                    randomUpgrade = Random.Range(0, 4);
                }
                
                switch (randomUpgrade)
                {
                    case 0: Attack += 5; break;
                    case 1: Defense += 4; break;
                    case 2: CriticalDamage += 0.1f; break;
                    case 3: CriticalRate += 5; break;
                }
                LevelPoints--;
            }
            HpMax = HP;
            Level++;
            _fillHealthBar.ModifySliderMaxValue(1);
            FillSliderValue();
            Exp = 0;
            GameManager.SharedInstance._playerLevelText.text = $"Lvl: {Level}";
            GameManager.SharedInstance._menuPlayerHPText.text = $"HP Max: {HpMax}";
            GameManager.SharedInstance._menuPlayerLevelText.text = $"Level: {Level}";
            GameManager.SharedInstance._menuPlayerAttackText.text = $"Attack: {Attack}";
            GameManager.SharedInstance._menuPlayerDefenseText.text = $"Defense: {Defense}";
            GameManager.SharedInstance._menuPlayerSpeedText.text = $"Speed: {Speed}";
            GameManager.SharedInstance._menuPlayerCriticalRateText.text = $"Critical Rate: {CriticalRate}%";
            GameManager.SharedInstance._menuPlayerCriticalDamageText.text = $"Critical Damage: {CriticalDamage * 100}%";
            //GameManager.SharedInstance.menuPlayerLevelPointsText.text = $"LP: {LevelPoints}"; 
        }
    }

    public bool IsCritical()
    {
        var isCritical = false;
        var random = Random.Range(0, 100);
        if (CriticalRate > random)
        {
            isCritical = true;
            Damage = (int)(Attack * CriticalDamage);
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
}
