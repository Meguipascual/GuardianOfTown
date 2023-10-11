using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PermanentPowerUps/DoubleShoot")]
public class DoubleShootPowerUp : PoweupEffect
{
    private LevelUpSliderManager _sliderManager;
    [SerializeField] private int _id;
    public int Id => _id;
    public override void Apply(GameObject target){}
    public void Apply()
    {
        _sliderManager = FindObjectOfType<LevelUpSliderManager>();
        PermanentPowerUpsSettings.Instance.ActivateDoubleShoot();
        _sliderManager.ContinueToLevelPointsButton();
    }
}
