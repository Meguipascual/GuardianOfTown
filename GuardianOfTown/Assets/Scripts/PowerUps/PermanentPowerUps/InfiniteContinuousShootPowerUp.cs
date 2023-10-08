using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PermanentPowerUps/InfiniteContinuousShoot")]
public class InfiniteContinuousShootPowerUp : PoweupEffect
{
    [SerializeField] private LevelUpSliderManager _sliderManager;
    public override void Apply(GameObject target){}
    public void Apply()
    {
        _sliderManager = FindObjectOfType<LevelUpSliderManager>();
        PermanentPowerUpsSettings.Instance.ActivateInfiniteContinuousShoot();
        _sliderManager.ContinueToLevelPointsButton();
    }
}
