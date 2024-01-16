using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ScriptableObjects/PowerUps/SpeedBuff")]
public class SpeedBuff : PoweupEffect
{
    [SerializeField] private float _amount;
    [SerializeField] private float _timerLimit;


    public override void Apply(GameObject target)
    {
        ActivateEffect();
    }

    private void ActivateEffect()
    {
        PowerUpSettings.Instance.ActivateSpeedTimer(_amount, _timerLimit);
    } 

}
