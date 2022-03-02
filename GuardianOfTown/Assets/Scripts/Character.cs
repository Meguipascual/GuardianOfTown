using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    protected abstract int Level { get; set; }
    public abstract int HP { get; set; }
    public abstract int Attack { get; set; }
    public abstract int Defense { get; set; }
    protected abstract float Speed { get; set; }

    public void ReceiveDamage(int damage)
    {
        HP -= damage;
    }

    public abstract void Death();

    public abstract void Move();
}
