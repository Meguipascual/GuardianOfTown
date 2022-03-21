using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcManager : Enemy
{    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Level = DataPersistentManager.Wave;
        Attack = 10;
        HP = 50;
        Defense = 10;
        Speed = 1.5f;
        Exp = 10;
        LevelUp();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Player.IsDead)
        {
            Move();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Trigger(other);
    }    
}
