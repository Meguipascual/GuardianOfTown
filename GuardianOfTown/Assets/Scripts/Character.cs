using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    protected int hp;
    public int Attack { get; set; }
    protected int defense;
    protected int level;
    protected float speed;

    protected abstract void ReceiveDamage(int damage);

    protected abstract void Death();
}
