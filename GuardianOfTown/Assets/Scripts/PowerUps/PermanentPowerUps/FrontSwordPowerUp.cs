using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PermanentPowerUps/FrontSword")]
public class FrontSwordPowerUp : PoweupEffect
{
    private LevelUpSliderManager _sliderManager;
    public override void Apply(GameObject target){}
    public void Apply()
    {
        _sliderManager = FindObjectOfType<LevelUpSliderManager>();
        PermanentPowerUpsSettings.Instance.ActivateSword();
        _sliderManager.ContinueToLevelPointsButton();
    }
}
