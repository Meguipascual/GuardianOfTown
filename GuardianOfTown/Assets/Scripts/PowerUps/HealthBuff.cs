using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUps/HealthBuff")]
public class HealthBuff : PoweupEffect
{
    [SerializeField] private float amount;
    public override void Apply(GameObject target)
    {
        var player = target.GetComponent<PlayerController>();

        amount *= player.HpMax;

        if ((player.HP + (int) amount) < player.HpMax)
        {
            Debug.Log("aumentar salud");
            player.HP += (int) amount;
        }
        else if(player.HP <= 0 )
        {
            Debug.Log("no hacer nada");
            return;
        }
        else
        {
            Debug.Log("salud maxima");
            player.HP = player.HpMax;
        }

        player.FillSliderValue();
    }

    public override void Move()
    {
        throw new System.NotImplementedException();
    }
}
