using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PermanentPowerUps/InfiniteContinuousShoot")]
public class InfiniteContinuousShootPowerUp : PoweupEffect
{
    public override void Apply(GameObject target){}
    public void Apply()
    {
        var permanentPowerUpsSettings = FindObjectOfType<PermanentPowerUpsSettings>();
        permanentPowerUpsSettings.ActivateInfiniteContinuousShoot();
    }
}
