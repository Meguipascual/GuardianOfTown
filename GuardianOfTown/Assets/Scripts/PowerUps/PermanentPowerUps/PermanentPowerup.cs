using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermanentPowerup : MonoBehaviour
{
    public PoweupEffect _powerupEffect;
    private PermanentPowerUpsSettings _permanentPowerUpsSettings;

    public void ActivatePowerup()
    {
        _powerupEffect.Apply(_permanentPowerUpsSettings.gameObject);
    }
}
