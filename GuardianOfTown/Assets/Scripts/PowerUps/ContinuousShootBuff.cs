using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PowerUps/ContinuousShootBuff")]
public class ContinuousShootBuff : PoweupEffect
{
    [SerializeField] private float _timerLimit;//10
    public override void Apply(GameObject target)
    {
        ActivateEffect();
    }

    private void ActivateEffect()
    {
        PowerUpSettings.Instance.ActivateBulletTimer(_timerLimit);
    }
}
