using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PermanentPowerUps/TripleShoot")]
public class TripleShootPowerUp : PoweupEffect
{
    private LevelUpSliderManager _sliderManager;
    [SerializeField] private int _id;
    public int Id => _id;
    public override void Apply(GameObject target){}
    public void Apply()
    {
        _sliderManager = FindObjectOfType<LevelUpSliderManager>();
        PermanentPowerUpsSettings.Instance.ActivateTripleShoot();
        _sliderManager.ContinueToLevelPointsButton();
    }
}
