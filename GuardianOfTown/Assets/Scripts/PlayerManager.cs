using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Character
{
    private GameManager gameManager;
    private int hpMax;
    private float xRange = 23f;
    private float horizontalInput;
    [SerializeField]private Vector3 offset = new Vector3(0, 0, 1);


    public bool IsDead { get; set; }
    public int TownHP { get; set; }
    public int Exp { get; set; }
    
    // Start is called before the first frame update
    void Start()
    {
        Level = 1;
        HP = 100;
        hpMax = HP;
        Attack = 10;
        Defense = 10;
        Speed = 10f;
        TownHP = 100;
        gameManager = FindObjectOfType<GameManager>();
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

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Orc"))
        {
            var enemy = other.GetComponent<OrcManager>();
            ReceiveDamage(enemy.Attack - (Defense / 2));
        }
        else if (other.CompareTag("Troll"))
        {
            var enemy = other.GetComponent<TrollManager>();
            ReceiveDamage(enemy.Attack - (Defense / 2));
        }
        else if (other.CompareTag("Goblin"))
        {
            var enemy = other.GetComponent<GoblinManager>();
            ReceiveDamage(enemy.Attack - (Defense / 2));
        }
        
        if (HP <= 0)
        {
            gameManager.playerHPText.text = "HP: 0";
            Die();
        }else
        {
            gameManager.playerHPText.text = "HP: " + HP;
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
            gameManager.townHPText.text = "Town Resistance: 0";
            Die();
        }
        else
        {
            gameManager.townHPText.text = "Town Resistance: " + TownHP;
        }
    }

    public override void LevelUp() 
    {
        HP = hpMax;
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
        hpMax = HP;
        Level++;
        gameManager.playerLevelText.text = "Lvl: " + Level;
        Exp = 0;
        Debug.Log(Level + ": Level\n" + Attack + ": Attack " + Defense + " :Defense");
    }
}
