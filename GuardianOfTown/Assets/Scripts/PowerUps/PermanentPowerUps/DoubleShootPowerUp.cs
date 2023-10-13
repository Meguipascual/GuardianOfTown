using UnityEngine;
using UnityEngine.UI;

public class DoubleShootPowerUp : PermanentPowerup
{
    private void Start()
    {
        _id = 4;
        base.Initialize();
        _thisButton = gameObject.GetComponent<Button>();
        if (_permanentPowerUpsSettings.IsDoubleShootActive)
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
        _permanentPowerUpsSettings.ActivateDoubleShoot();
        _permanentPowerUpManager._notUsedPowerUpPrefabs.RemoveAt(Index);
        _sliderManager.ContinueToLevelPointsButton();
    }
}
