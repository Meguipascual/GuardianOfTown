using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Character
{
    private float xRange = 23f;
    private float horizontalInput;
    private FillHealthBar fillHealthBar;
    [SerializeField]private Vector3 offset = new Vector3(0, 0, 1);

    public bool IsDead { get; set; }
    public int TownHP { get; set; }
    public int Exp { get; set; }
    public int HpMax { get; set; }
    
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

    public void TownReceiveDamage(int damage)
    {

        if(damage > 0)
        {
            TownHP -= damage;
        }
        if (TownHP <= 0)
        {
            GameManager.SharedInstance.townHPText.text = "Town Resistance: 0";
            Die();
        }
        else
        {
            GameManager.SharedInstance.townHPText.text = "Town Resistance: " + TownHP;
        }
    }

    public override void LevelUp() 
    {
        HP = HpMax;
        for (int i = 0; i < 2; i++)
        {
            HP += 10;
            var randomUpgrade = Random.Range(0, 2);
            switch (randomUpgrade)
            {
                case 0: Attack += 5; break;
                case 1: Defense += 4; break;
            }
        }
        HpMax = HP;
        Level++;
        fillHealthBar.ModifySliderMaxValue(1);
        fillHealthBar.FillSliderValue();
        Exp = 0;
        GameManager.SharedInstance.playerLevelText.text = "Lvl: " + Level;
        GameManager.SharedInstance.menuPlayerLevelText.text = $"Level: {Level}";
        GameManager.SharedInstance.menuPlayerAttackText.text = $"Attack: {Attack}";
        GameManager.SharedInstance.menuPlayerDefenseText.text = $"Defense: {Defense}";
        GameManager.SharedInstance.menuPlayerSpeedText.text = $"Speed: {Speed}";
    }
}
