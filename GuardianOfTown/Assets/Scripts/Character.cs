using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public abstract int Level { get; set; }
    public abstract int HP { get; set; }
    public abstract int Attack { get; set; }
    public abstract int Defense { get; set; }
    protected abstract float Speed { get; set; }

    public void ReceiveDamage(int damage)
    {
        if (damage > 0)
        {
            HP -= damage;
        }
        
    }

    public virtual void LevelUp() { }

    public abstract void Death();

    public abstract void Move();
}
