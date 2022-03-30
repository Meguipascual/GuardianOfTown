using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : Character
{
    private float xRange = 23f;
    private float horizontalInput;
    private FillHealthBar fillHealthBar;
    [SerializeField]private Vector3 offset = new Vector3(0, 0, 1);

    public bool IsDead { get; set; }
    public int Exp { get; set; }
    public int CriticalRate { get; set; }
    public int Damage { get; set; }
    public float CriticalDamage { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        fillHealthBar = FindObjectOfType<FillHealthBar>();
        DataPersistantManager.Instance.LoadPlayerStats();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsDead)
        {
            Move();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Get an object object from the pool
                GameObject pooledProjectile = ObjectPooler.SharedInstance.GetPooledObject();
                if (pooledProjectile != null)
                {
                    pooledProjectile.SetActive(true); // activate it
                    pooledProjectile.transform.position = transform.position + offset; // position it at player
                    ObjectPooler.ProjectileCount--;
                    GameManager.SharedInstance.projectileText.text = "Projectile: " + ObjectPooler.ProjectileCount;
                }
            }
        }
    }

    public override void Die()
    {
        
        Debug.Log("GameOver");
        IsDead = true;
        //GameOver
    }

    public override void Move()
    {
        // Check for left and right bounds
        if (transform.position.x < -xRange)
        {
            transform.position = new Vector3(-xRange, transform.position.y, transform.position.z);
        }

        if (transform.position.x > xRange)
        {
            transform.position = new Vector3(xRange, transform.position.y, transform.position.z);
        }

        // Player movement left to right
        horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * Time.deltaTime * Speed * horizontalInput);
        
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
        var shields = GameManager.SharedInstance.TownHpText.GetComponentsInChildren(
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
            HP = HpMax;
            for (int i = 0; i < 2; i++)
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
            }
            HpMax = HP;
            Level++;
            fillHealthBar.ModifySliderMaxValue(1);
            FillSliderValue();
            Exp = 0;
            GameManager.SharedInstance.playerLevelText.text = "Lvl: " + Level;
            GameManager.SharedInstance.menuPlayerLevelText.text = $"Level: {Level}";
            GameManager.SharedInstance.menuPlayerAttackText.text = $"Attack: {Attack}";
            GameManager.SharedInstance.menuPlayerDefenseText.text = $"Defense: {Defense}";
            GameManager.SharedInstance.menuPlayerSpeedText.text = $"Speed: {Speed}";
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
