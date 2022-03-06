using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public int Level { get; set; }
    public int HP { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    protected float Speed { get; set; }

    public void ReceiveDamage(int damage)
    {
        if (damage > 0)
        {
            HP -= damage;
        }
        
    }

    public virtual void LevelUp() { }

    public abstract void Die();

    public abstract void Move();
}
