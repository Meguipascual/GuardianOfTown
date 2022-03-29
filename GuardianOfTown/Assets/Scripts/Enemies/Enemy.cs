using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public abstract class Enemy : Character
{
    private FillEnemyHealthBar fillEnemyHealthBar;
    protected DataPersistantManager DataPersistentManager { get; set; }
    protected int Exp { get; set; }
    protected PlayerController Player { get; set; }

    protected virtual void Start()
    {
        Player = FindObjectOfType<PlayerController>();
        DataPersistentManager = FindObjectOfType<DataPersistantManager>();
        fillEnemyHealthBar = GetComponentInChildren<FillEnemyHealthBar>();
        fillEnemyHealthBar.slider.gameObject.SetActive(false);
    }
    protected void Trigger (Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            other.gameObject.SetActive(false);
            ObjectPooler.ProjectileCount++; 
            var damage = Player.Attack - (Defense / 2); 
            ReceiveDamage(damage);
            fillEnemyHealthBar.slider.gameObject.SetActive(true);
            fillEnemyHealthBar.FillEnemySliderValue();
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
            Player.TownReceiveDamage(); 
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
            HpMax += 10;
            HP = HpMax;
            var randomUpgrade = Random.Range(0, 2);
            switch (randomUpgrade)
            {
                case 0: Attack += 5; break;
                case 1: Defense += 4; break;
            }
        }
    }
}
