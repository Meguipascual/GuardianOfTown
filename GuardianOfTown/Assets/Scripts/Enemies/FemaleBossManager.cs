using System.Collections;
using UnityEngine;

public class FemaleBossManager : Enemy
{
    [SerializeField] private GameObject floatingTextPrefab;
    [SerializeField] private GameObject criticalHitTextPrefab;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Level = SpawnManager.LevelOfBosses;
        Attack = 100;
        HP = 500;
        HpMax = HP;
        Defense = 10;
        Speed = 0.5f;
        Exp = 50;
        TimeToMove = 2f;
        TimeToRest = 2f;
        EnemyMove = "FemaleBossMove";
        EnemyDeath = "FemaleBossDeath";
        DeathDelay = 1.2f;
        LevelUp();
    }

    // Update is called once per frame
    void Update()
    {
        if(Player.IsDead) return;

        TryToMove();
    }

    private void OnTriggerEnter(Collider other)
    {
        Trigger(other, floatingTextPrefab, criticalHitTextPrefab);
    }   
}
