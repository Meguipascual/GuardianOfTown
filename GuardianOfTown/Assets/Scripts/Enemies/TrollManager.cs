using UnityEngine;

public class TrollManager : Enemy
{
    [SerializeField] private GameObject floatingTextPrefab;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Level = DataPersistentManager.Wave;
        Attack = 20;
        HP = 100;
        HpMax = HP;
        Defense = 10;
        Speed = 1f;
        Exp = 20;
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
        Trigger(other, floatingTextPrefab);
    }
}
