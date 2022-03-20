using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Enemy : Character
{
    
    private GameManager gameManager;
    private DataPersistantManager dataPersistantManager;
    protected int Exp { get; set; }
    protected PlayerManager Player { get; set; }

    protected virtual void Start()
    {
        Player = FindObjectOfType<PlayerManager>();
        gameManager = FindObjectOfType<GameManager>();
        dataPersistantManager = FindObjectOfType<DataPersistantManager>();
    }
    protected void Trigger (Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            other.gameObject.SetActive(false);
            var damage = Player.Attack - (Defense / 2); 
            ReceiveDamage(damage);
            Debug.Log("ouch, it hurts" + HP);
            if (HP <= 0)
            {
                Die();
                Player.Exp += Exp;
                if(Player.Exp > 20)
                {
                    Player.LevelUp();
                }
            }
        }
        else if (other.CompareTag("Wall"))
        {
            Die();
            Player.TownReceiveDamage(Attack); 
        }
        else if (other.CompareTag("Player"))
        {
            Player.Exp += Exp;
            if (Player.Exp > 20)
            {
                Player.LevelUp();
                gameManager.playerHPText.text = "HP: " + Player.HP;
            }
            Die();
        }
    }

    public override void Die()
    {
        if (this.CompareTag("Boss"))
        {
            dataPersistantManager.ChangeStage();
        }
        Destroy(gameObject);
    }
    public override void Move()
    {
        transform.position += Vector3.back * Time.deltaTime * Speed;
    }
    public override void LevelUp()
    {
        for (int i = 0; i < Level * 2; i++)
        {
            HP += 10;
            var randomUpgrade = Random.Range(0, 2);
            switch (randomUpgrade)
            {
                case 0: Attack += 5; break;
                case 1: Defense += 4; break;
            }
        }
    }
}
