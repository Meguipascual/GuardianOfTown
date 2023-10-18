using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public abstract class Enemy : Character
{
    private FillEnemyHealthBar _fillEnemyHealthBar;
    private ParticleSystem _criticalHitParticleSystem;
    private Animator _animator;
    private Collider _collider;
    [SerializeField] private GameObject _enemyPanelTop;
    [SerializeField] private GameObject _enemyPanelFront;
    protected DataPersistantManager DataPersistentManager { get; set; }
    protected SpawnManager SpawnManager { get; set; }
    protected int Exp { get; set; }
    protected PlayerController Player { get; set; }

    protected string EnemyMove { get; set; }
    protected string EnemyDeath { get; set; }

    protected float TimeToRest { get; set; }
    protected float DeathDelay { get; set; }
    protected float TimeToMove { get; set; }

    private float MovementTimer { get; set; }
    private float RestingTimer { get; set; }
    private bool IsResting { get; set; }
    private bool IsDying { get; set; }

    protected virtual void Start()
    {
        if (GameSettings.Instance.IsTopViewModeActive)
        {
            _enemyPanelTop.gameObject.SetActive(true);
            _enemyPanelFront.gameObject.SetActive(false);
        }
        else
        {
            _enemyPanelTop.gameObject.SetActive(false);
            _enemyPanelFront.gameObject.SetActive(true);
        }
        Player = FindObjectOfType<PlayerController>();
        DataPersistentManager = FindObjectOfType<DataPersistantManager>();
        SpawnManager = FindObjectOfType<SpawnManager>();
        _fillEnemyHealthBar = GetComponentInChildren<FillEnemyHealthBar>();
        _criticalHitParticleSystem = GetComponentInChildren<ParticleSystem>();
        _animator = GetComponentInChildren<Animator>();
        _collider = GetComponent<Collider>();
        _fillEnemyHealthBar.slider.gameObject.SetActive(false);
        
    }
    protected void Trigger (Collider other, GameObject floatingTextPrefab, GameObject criticalHitPrefab)
    {
        if (other.CompareTag(Tags.Bullet))
        {
            CollisionWithBullet(other,floatingTextPrefab,criticalHitPrefab);
        }
        else if (other.CompareTag(Tags.Wall))
        {
            CollisionWithWall();
        }
        else if (other.CompareTag(Tags.Barrier))
        {
            CollisionWithBarrier();
        }
        else if (other.CompareTag(Tags.Sword))
        {
            CollisionWithSword();
            Debug.Log($"Sword");
        }
        else if (other.CompareTag(Tags.Player))
        {
            CollisionWithPlayer();
            Debug.Log($"Player");
        }
    }

    public override void Die()
    {
        GameManager.SharedInstance.DecreaseNumberOfEnemies();
        Destroy(gameObject);
    }

    IEnumerator DieInSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Die();
    }

    public override void TryToMove()
    {
        if (IsDying || GameManager.SharedInstance.IsGamePaused) return;

        if (IsResting)
        {
            if(RestingTimer >= 0)
            {
                RestingTimer -= Time.deltaTime;
            }
            else
            {
                RestingTimer = TimeToRest;
                IsResting = false;
            }
        }
        else if(MovementTimer >= 0)
        {
            if (!_animator.GetCurrentAnimatorStateInfo(0).IsName(EnemyMove)) 
            {
                _animator.Play(EnemyMove);
            } 
            Advance();
            MovementTimer -= Time.deltaTime;
        }
        else
        {
            IsResting = true;
            MovementTimer = TimeToMove;
        }
        
    }
    private void Advance()
    {
        transform.position += Vector3.back * Time.deltaTime * Speed;
        
    }
    public override void LevelUp()
    {
        var randomUpgrade = Random.Range(0, Level * 2);
        Attack += randomUpgrade * 5;
        Defense += (Level * 2) - randomUpgrade;
        HpMax += 20 * Level;
        HP = HpMax;
    }
    
    private void CollisionWithBullet(Collider other, GameObject floatingTextPrefab, GameObject criticalHitPrefab)
    {
        var damage = 0;
        var critical = false;

        if (Player.IsCritical())
        {
            _criticalHitParticleSystem.Play();
            GameManager.SharedInstance.ShakeCamera();
            critical = true;
        }

        other.GetComponent<BulletManager>().DestroyBullet(other.gameObject);
        damage = Player.Damage - (Defense / 2);
        ShowDamage(critical, damage, floatingTextPrefab, criticalHitPrefab);
        ReceiveDamage(damage);
        _fillEnemyHealthBar.slider.gameObject.SetActive(true);
        _fillEnemyHealthBar.FillEnemySliderValue();

        if (HP <= 0)
        {
            Player.Exp += Exp;
            //Player.LevelUp();
            if(EnemyDeath != null)
            {
                IsDying = true;
                _collider.enabled = false;
                _fillEnemyHealthBar.slider.gameObject.SetActive(false);
                StartCoroutine(DieInSeconds(DeathDelay));
                _animator.Play(EnemyDeath);
            }
            else
            {
                Die();
            } 
        }
    }

    private void CollisionWithWall()
    {
        if (CompareTag(Tags.Boss))
        {
            Player.Die();
            return;
        }
        Player.TownReceiveDamage();
        Die();
    }

    private void CollisionWithBarrier()
    {
        PermanentPowerUpsSettings.Instance.DeactivateTownBarrier();
        Die();
    }

    private void CollisionWithSword()
    {
        if (CompareTag(Tags.Boss))
        {
            PermanentPowerUpsSettings.Instance.DeactivateSword();
        }
        //Player.shieldParticleSystem.Play();//Change to a sword particle or something
        Player.Exp += Exp;
        Die();
    }

    private void CollisionWithPlayer()
    {
        Player.ReceiveDamage(Attack - (Player.Defense / 2));
        Player.ComprobateLifeRemaining();

        if (!Player.IsDead)
        {
            Player.shieldParticleSystem.Play();
            Player.Exp += Exp;
            Die();
        }
    }

    void ShowDamage(bool isCritical, int damage, GameObject floatingTextPrefab, GameObject criticalHitPrefab)
    {
        
        GameObject prefab = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity);
        if (damage < 0)
        {
            damage = 0;
        }

        prefab.GetComponentInChildren<TextMesh>().text = damage.ToString();
                
        if (isCritical)
        {
            Instantiate(criticalHitPrefab, transform.position, Quaternion.identity);
            prefab.GetComponentInChildren<TextMesh>().color = Color.yellow;
        }
    }
}
