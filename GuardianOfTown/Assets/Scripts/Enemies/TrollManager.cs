using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollManager : Enemy
{
    protected override PlayerManager player { get; set; }
    protected override GameManager gameManager { get; set; }
    public override int Level { get; set; }
    public override int HP { get; set; }
    public override int Attack { get; set; }
    public override int Defense { get; set; }
    protected override float Speed { get; set; }
    protected override int Exp { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        Level = 0;
        Attack = 20;
        HP = 100;
        Defense = 0;
        Speed = 1f;
        Exp = 20;
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
