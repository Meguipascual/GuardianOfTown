using UnityEngine;
using UnityEngine.UI;

public class InfiniteContinuousShootPowerUp : PermanentPowerup
{
    private void Start()
    {
        _id = 3;
        base.Start();
        _thisButton = gameObject.GetComponent<Button>();
        if (_permanentPowerUpsSettings.IsInfiniteContinuousShootActive)
        {
            _thisButton.interactable = false;
        }
        else
        {
            _thisButton.interactable = true;
        }
    }
    public void Apply()
    {
        _permanentPowerUpsSettings.ActivateInfiniteContinuousShoot();
        _permanentPowerUpManager._notUsedPowerUpPrefabs.RemoveAt(Index);
        _sliderManager.ContinueToLevelPointsButton();
    }
}
