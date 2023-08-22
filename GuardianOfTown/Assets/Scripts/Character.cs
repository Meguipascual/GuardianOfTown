using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public int Level { get; set; }//Encapsulation
    public int HP { get; set; }//Encapsulation
    public int HpMax { get; set; }//Encapsulation
    public int Attack { get; set; }//Encapsulation
    public int Defense { get; set; }//Encapsulation
    public float Speed { get; set; }//Encapsulation

    public void ReceiveDamage(int damage)//Abstraction
    {
        if (damage > 0)
        {
            HP -= damage;
        }
        
    }

    public virtual void LevelUp() { }

    public abstract void Die();

    public abstract void TryToMove();
}
