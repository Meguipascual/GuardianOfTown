using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    protected int hp;
    protected int attack;
    protected int defense;
    protected int level;
    protected float speed;

    protected abstract void ReceiveDamage(int damage);

    virtual public void Death()
    {
        //If Enemy then Destroy(), if playerthen GameOver
    }
}
