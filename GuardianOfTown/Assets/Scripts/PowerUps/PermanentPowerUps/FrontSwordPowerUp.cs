using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PermanentPowerUps/FrontSword")]
public class FrontSwordPowerUp : PoweupEffect
{
    public override void Apply(GameObject target){}
    public void Apply()
    {
        PermanentPowerUpsSettings.Instance.ActivateSword();
    }
}
