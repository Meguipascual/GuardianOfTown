using UnityEngine;

public class BossManager : Enemy
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Level = DataPersistentManager.Wave;
        Attack = 100;
        HP = 500;
        Defense = 20;
        Speed = .5f;
        Exp = 50;
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
        if (other.CompareTag("Wall"))
        {
            Player.TownReceiveDamage(Attack);
        }
        else
        {
            Trigger(other);
        }
        
        
    }
}
