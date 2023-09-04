using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerController : Character
{
    public float XLeftBound { get; set; }
    public float XRightBound { get; set; }
    private float horizontalInput;
    private FillHealthBar fillHealthBar;
    public ParticleSystem shieldParticleSystem;
    [SerializeField]private Vector3 offset = new Vector3(0, 0, 1);
    [SerializeField] private int _levelPoints;
    [SerializeField] private float _bulletTimeCounter;
    [SerializeField] private float _bulletDelay;
    [SerializeField] private bool _isCannonOverheated;
    [SerializeField] private float _cannonOverHeatedTimer;
    [SerializeField] private float _cannonOverHeatedLimit;
    [SerializeField] private float _coolDownDelay;

    public bool IsDead { get; set; }
    public int Exp { get; set; }
    public int LevelPoints { get; set; }
    public int CriticalRate { get; set; }
    public int Damage { get; set; }
    public float CriticalDamage { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        _bulletTimeCounter = 0;
        _bulletDelay = 0.2f;
        _cannonOverHeatedLimit = 30f;
        _cannonOverHeatedTimer = 0;
        _coolDownDelay = 5f;
        XLeftBound = DataPersistantManager.Instance.SpawnBoundariesLeft[0];
        XRightBound = DataPersistantManager.Instance.SpawnBoundariesRight[0];
        shieldParticleSystem = GetComponentInChildren<ParticleSystem>();
        fillHealthBar = FindObjectOfType<FillHealthBar>();
        DataPersistantManager.Instance.LoadPlayerStats();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDead)
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
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                Shoot();
            }
        }
    }

    private void ShootEasyMode()
    {
        if (_isCannonOverheated || (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.W)))
        {
            _cannonOverHeatedTimer -= Time.deltaTime;
            if (_cannonOverHeatedTimer < 0)
            {
                _isCannonOverheated = false;
            }
            return;
        }

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            _bulletTimeCounter += Time.deltaTime;
            _cannonOverHeatedTimer += Time.deltaTime;
            if (_bulletTimeCounter > _bulletDelay)
            {
                _bulletTimeCounter = 0;
                Shoot();
            }
            if (_cannonOverHeatedTimer > _cannonOverHeatedLimit)
            {
                OverHeatedManager.Instance.ChangeCannonMaterial(100);
                _isCannonOverheated = true;
                _cannonOverHeatedTimer = _coolDownDelay;
            }
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
            GameManager.SharedInstance._projectileText.text = "Projectile: " + ObjectPooler.ProjectileCount;
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
        horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * Time.deltaTime * Speed * horizontalInput);
        
    }

    IEnumerator CoolDownCannon()
    {
        yield return new WaitForSeconds(_cannonOverHeatedLimit);
        _cannonOverHeatedTimer = 0;
        _isCannonOverheated = false;
    }

    public void ComprobateLifeRemaining ()
    {
        fillHealthBar.FillSliderValue();
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
            fillHealthBar.ModifySliderMaxValue(1);
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
        fillHealthBar.FillSliderValue();
    }
}
