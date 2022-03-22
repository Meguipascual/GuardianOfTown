using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Enemy : Character
{
    protected DataPersistantManager DataPersistentManager { get; set; }
    protected int Exp { get; set; }
    protected PlayerController Player { get; set; }

    protected virtual void Start()
    {
        Player = FindObjectOfType<PlayerController>();
        DataPersistentManager = FindObjectOfType<DataPersistantManager>();
    }
    protected void Trigger (Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            other.gameObject.SetActive(false);
            ObjectPooler.ProjectileCount++;
            GameManager.SharedInstance.projectileText.text = $"Projectile: {ObjectPooler.ProjectileCount}"; 
            var damage = Player.Attack - (Defense / 2); 
            ReceiveDamage(damage);
            Debug.Log("ouch, it hurts" + HP);
            if (HP <= 0)
            {
                Player.Exp += Exp;
                if(Player.Exp > 20)
                {
                    Player.LevelUp();
                }
                Die();
            }
        }
        else if (other.CompareTag("Wall"))
        {
            Die();
            Player.TownReceiveDamage(Attack); 
        }
        else if (other.CompareTag("Player"))
        {
            Player.ReceiveDamage(Attack - (Player.Defense / 2));
            Player.ComprobateLifeRemaining();
            if (!Player.IsDead)
            {
                Player.Exp += Exp;
                if (Player.Exp > 20)
                {
                    Player.LevelUp();
                    GameManager.SharedInstance.playerHPText.text = "HP: " + Player.HP;
                }
                Die();
            }
        }
    }

    public override void Die()
    {
        if (this.CompareTag("Boss"))
        {
            if (Player.Exp > 20)
            {
                Player.LevelUp();
                GameManager.SharedInstance.playerHPText.text = "HP: " + Player.HP;
            }
            DataPersistentManager.ChangeStage();
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
