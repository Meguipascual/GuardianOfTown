using UnityEngine;

public class TrollManager : Enemy
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Level = 0;
        Attack = 20;
        HP = 100;
        Defense = 0;
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
        Trigger(other);
    }
}
