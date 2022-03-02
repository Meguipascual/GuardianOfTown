using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    private string enemyType;
    public GameObject player;
    

    // Start is called before the first frame update
    void Start()
    {
        speed = 10f;
        hp = 100;
        Attack = 30;
        enemyType = "Orc";
        ReceiveDamage(10);
        Debug.Log(hp + ": " + enemyType);
    }
    private void Update()
    {
        transform.position = transform.position + Vector3.back * Time.deltaTime * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            ReceiveDamage(10);
        }
        else if (other.CompareTag("Wall"))
        {
            Death(); 
        }
    }

    protected override void Death()
    {
        Destroy(gameObject);
    }

    protected override void ReceiveDamage(int damage)
    {
        hp -= damage;
    }
}
