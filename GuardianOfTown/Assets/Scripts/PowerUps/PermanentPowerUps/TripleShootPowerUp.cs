using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PermanentPowerUps/TripleShoot")]
public class TripleShootPowerUp : PoweupEffect
{
    [SerializeField] private LevelUpSliderManager _sliderManager;
    public override void Apply(GameObject target){}
    public void Apply()
    {
        _sliderManager = FindObjectOfType<LevelUpSliderManager>();
        PermanentPowerUpsSettings.Instance.ActivateTripleShoot();
        _sliderManager.ContinueToLevelPointsButton();
    }
}
