using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    static int hp;
    static int attack;
    static int defense;
    static int level;
    
    static void  ReceiveDamage(int damage)
    {
        hp -= damage;
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        //if bullet then ReceiveDamage, if end of zone then Destroy and Gameover
    }

    public virtual void Death()
    {
        //If Enemy then Destroy(), if playerthen GameOver
    }
}
