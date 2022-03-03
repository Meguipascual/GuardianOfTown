using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Character
{
    public override int Level { get; set; }
    public override int HP { get; set; }
    public override int Attack { get; set; }
    public override int Defense { get; set; }
    protected override float Speed { get; set; }
    public bool IsDead { get; set; }
    public int TownHP { get; set; }
    public int Exp { get; set; }
    private GameManager gameManager;
    private int hpMax;

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
        
    }

    public override void Death()
    {
        
        Debug.Log("GameOver");
        IsDead = true;
        //GameOver
    }

    public override void Move()
    {
        throw new System.NotImplementedException();
        //axis mover derecha izquierda
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
            Death();
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
