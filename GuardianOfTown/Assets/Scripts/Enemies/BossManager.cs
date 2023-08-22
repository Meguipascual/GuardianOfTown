using System.Collections;
using UnityEngine;

public class BossManager : Enemy
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
        EnemyMove = "EnemyMove";
        LevelUp();
    }

    // Update is called once per frame
    void Update()
    {
        if(Player.IsDead) return;

        Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        Trigger(other, floatingTextPrefab, criticalHitTextPrefab);
    }   
}
