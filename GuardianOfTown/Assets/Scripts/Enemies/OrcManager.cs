using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcManager : Enemy
{
    [SerializeField] private GameObject floatingTextFrontPrefab;
    [SerializeField] private GameObject criticalHitTextFrontPrefab;
    [SerializeField] private GameObject floatingTextTopPrefab;
    [SerializeField] private GameObject criticalHitTextTopPrefab;

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
        EnemyDeath = "OrcDeath";
        DeathDelay = 0.4f;
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
        if (GameSettings.Instance.IsTopViewModeActive)
        {
            Trigger(other, floatingTextTopPrefab, criticalHitTextTopPrefab);
        }
        else
        {
            Trigger(other, floatingTextFrontPrefab, criticalHitTextFrontPrefab);
        }
    }
}
