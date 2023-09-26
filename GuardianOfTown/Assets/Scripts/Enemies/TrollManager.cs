using UnityEngine;

public class TrollManager : Enemy
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
        Attack = 20;
        HP = 100;
        HpMax = HP;
        Defense = 10;
        Speed = 1f;
        Exp = 20;
        TimeToMove = 1.5f;
        TimeToRest = 1.5f;
        EnemyMove = "TrollMove";
        EnemyDeath = "TrollDeath";
        DeathDelay = 1.25f;
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
