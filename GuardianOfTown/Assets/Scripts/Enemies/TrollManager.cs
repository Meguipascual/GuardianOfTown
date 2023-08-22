using UnityEngine;

public class TrollManager : Enemy
{
    [SerializeField] private GameObject floatingTextPrefab;
    [SerializeField] private GameObject criticalHitTextPrefab;
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
        TimeToMove = 2f;
        TimeToRest = 2f;
        LevelUp();
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.IsDead) return;

        Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        Trigger(other, floatingTextPrefab, criticalHitTextPrefab);
    }
}
