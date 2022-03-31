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
        var amountToIncrement = amount;
        amountToIncrement *= player.HpMax;

        if ((player.HP + (int) amountToIncrement) < player.HpMax)
        {
            Debug.Log("aumentar salud");
            player.HP += (int) amountToIncrement;
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
}
