using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PermanentPowerUps/DoubleShoot")]
public class DoubleShootPowerUp : PoweupEffect
{
    public override void Apply(GameObject target){}
    public void Apply()
    {
        var permanentPowerUpsSettings = FindObjectOfType<PermanentPowerUpsSettings>();
        permanentPowerUpsSettings.ActivateDoubleShoot();
    }
}
