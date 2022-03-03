using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcManager : Enemy
{
    public override int Level { get; set; }
    public override int HP { get; set; }
    public override int Attack { get; set; }
    public override int Defense { get; set; }
    protected override float Speed { get; set; }
    protected override PlayerManager player { get; set; }
    protected override GameManager gameManager { get; set; }
    protected override int Exp { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        Level = 0;
        Attack = 10;
        HP = 50;
        Defense = 10;
        Speed = 1.5f;
        Exp = 10;
        LevelUp();
        player = FindObjectOfType<PlayerManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.IsDead)
        {
            Move();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Trigger(other);
    }    
}
