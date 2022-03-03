using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Enemy : Character
{ 
    protected abstract PlayerManager player { get; set; }
    protected abstract GameManager gameManager { get; set; }
    protected abstract int Exp { get; set; }
    public override abstract int Level { get; set; }
    public override abstract int HP { get; set; }
    public override abstract int Attack { get; set; }
    public override abstract int Defense { get; set; }
    protected override abstract float Speed { get; set; }

    protected void Trigger (Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            var damage = player.Attack - (Defense / 2); 
            ReceiveDamage(damage);
            if (HP <= 0)
            {
                Death();
                player.Exp += Exp;
                if(player.Exp > 20)
                {
                    player.LevelUp();
                }
            }
        }
        else if (other.CompareTag("Wall"))
        {
            Death();
            player.TownReceiveDamage(Attack);
            
            if (player.TownHP <= 0)
            {
                gameManager.townHPText.text = "Town Resistance: 0";
                player.Death();
            }
            else
            {
                gameManager.townHPText.text = "Town Resistance: " + player.TownHP;
            }
        }
        else if (other.CompareTag("Player"))
        {
            player.Exp += Exp;
            if (player.Exp > 20)
            {
                player.LevelUp();
                gameManager.playerHPText.text = "HP: " + player.HP;
            }
            Death();
        }
    }

    public override void Death()
    {
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
