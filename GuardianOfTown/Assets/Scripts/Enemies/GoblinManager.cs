using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinManager : Enemy
{
    [SerializeField] private GameObject floatingTextPrefab;
    [SerializeField] private GameObject criticalHitTextPrefab;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Level = SpawnManager.LevelOfEnemies;
        Attack = 5;
        HP = 30;
        HpMax = HP;
        Defense = 5;
        Speed = 3f;
        Exp = 5;
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
        Trigger(other, floatingTextPrefab, criticalHitTextPrefab);
    }
}
