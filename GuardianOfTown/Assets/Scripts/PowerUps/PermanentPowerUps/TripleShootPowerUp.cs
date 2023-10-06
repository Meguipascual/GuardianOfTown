using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PermanentPowerUps/TripleShoot")]
public class TripleShootPowerUp : PoweupEffect
{
    public override void Apply(GameObject target){}
    public void Apply()
    {
        var permanentPowerUpsSettings = FindObjectOfType<PermanentPowerUpsSettings>();
        permanentPowerUpsSettings.ActivateTripleShoot();
    }
}
