using UnityEngine;
using UnityEngine.UI;

public class OverHeatingUnactivePowerUp : PermanentPowerup
{
    private void Start()
    {
        _id = 0;
        base.Initialize();
        _thisButton = gameObject.GetComponent<Button>();
        if (_permanentPowerUpsSettings.IsOverHeatingUnactive)
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
        _permanentPowerUpsSettings.DeactivateOverHeating();
        _permanentPowerUpManager._notUsedPowerUpPrefabs.RemoveAt(Index);
        _sliderManager.ContinueToLevelPointsButton();
    }
}
