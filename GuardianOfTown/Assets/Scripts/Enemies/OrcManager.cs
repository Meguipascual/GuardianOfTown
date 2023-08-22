using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcManager : Enemy
{
    [SerializeField] private GameObject floatingTextPrefab;
    [SerializeField] private GameObject criticalHitTextPrefab;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Level = SpawnManager.LevelOfEnemies;
        Attack = 10;
        HP = 50;
        HpMax = HP;
        Defense = 10;
        Speed = 1.5f;
        Exp = 10;
        TimeToMove = 1f;
        TimeToRest = 1f;
        EnemyMove = "OrcMove";
        LevelUp();
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.IsDead) return;

        TryToMove();
    }

    private void OnTriggerEnter(Collider other)
    {
        Trigger(other, floatingTextPrefab, criticalHitTextPrefab);
    }    
}
