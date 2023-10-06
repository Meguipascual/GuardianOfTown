using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PermanentPowerUps/FrontSword")]
public class FrontSwordPowerUp : PoweupEffect
{
    public override void Apply(GameObject target){}
    public void Apply()
    {
        var permanentPowerUpsSettings = FindObjectOfType<PermanentPowerUpsSettings>();
        permanentPowerUpsSettings.ActivateSword();
    }
}
