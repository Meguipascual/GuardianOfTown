using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    private string enemyType;
    public PlayerManager player;

    protected override int Level { get; set; }
    public override int HP { get; set; }
    public override int Attack { get; set; }
    public override int Defense { get; set; }
    protected override float Speed { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        enemyType = "Orc";
        Level = 1;
        HP = 100;
        Attack = 15;
        Defense = 10;
        Speed = 10f;
        player = FindObjectOfType<PlayerManager>();
    }
    private void Update()
    {
        Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            var damage = player.Attack - (Defense / 2); 
            ReceiveDamage(damage);
            if (HP <= 0)
            {
                Death();
            }
        }
        else if (other.CompareTag("Wall"))
        {
            Death();
            player.ReceiveDamage(Attack);
            Debug.Log(player.HP);
            if (player.HP <= 0)
            {
                player.Death();
            }
        }
        else if (other.CompareTag("Player"))
        {
            Death();
        }
    }

    public override void Death()
    {
        Destroy(gameObject);
    }

    public override void Move()
    {
        transform.position = transform.position + Vector3.back * Time.deltaTime * Speed;
    }
}
