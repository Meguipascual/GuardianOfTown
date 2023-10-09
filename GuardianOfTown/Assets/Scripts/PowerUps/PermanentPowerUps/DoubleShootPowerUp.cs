using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PermanentPowerUps/DoubleShoot")]
public class DoubleShootPowerUp : PoweupEffect
{
    private LevelUpSliderManager _sliderManager;
    public override void Apply(GameObject target){}
    public void Apply()
    {
        _sliderManager = FindObjectOfType<LevelUpSliderManager>();
        PermanentPowerUpsSettings.Instance.ActivateDoubleShoot();
        _sliderManager.ContinueToLevelPointsButton();
    }
}
