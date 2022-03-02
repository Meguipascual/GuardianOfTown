using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Character
{
    protected override void Death()
    {
        //GameOver settings
    }

    protected override void ReceiveDamage(int damage)
    {
        hp -= damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        ReceiveDamage(other.gameObject.GetComponent<Enemy>().Attack);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
