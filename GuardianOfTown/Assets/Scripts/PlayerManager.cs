using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Character
{
    protected override int Level { get; set; }
    public override int HP { get; set; }
    public override int Attack { get; set; }
    public override int Defense { get; set; }
    protected override float Speed { get; set; }
    public bool IsDead { get; set; }
    public int TownHP { get; set; }
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        Level = 1;
        HP = 100;
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
        TownHP -= damage;
    }
}
