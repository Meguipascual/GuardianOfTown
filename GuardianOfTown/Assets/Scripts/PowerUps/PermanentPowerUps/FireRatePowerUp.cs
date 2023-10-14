using UnityEngine;
using UnityEngine.UI;

public class FireRatePowerUp : PermanentPowerup
{
    private void Start()
    {
        _id = 0;
        base.Initialize();
        _thisButton = gameObject.GetComponent<Button>();

        for (int i = 0; i < _permanentPowerUpsSettings.AreFireRateIncrementsWasted.Length; i++)
        {
            if (!_permanentPowerUpsSettings.AreFireRateIncrementsWasted[i])
            {
                _thisButton.interactable = true;
                return;
            }
        }

        _thisButton.interactable = false;
    }
    public void Apply()
    {
        _permanentPowerUpsSettings.ActivateFireRateIncrement();
        _permanentPowerUpManager._notUsedPowerUpPrefabs.RemoveAt(Index);
        _sliderManager.ContinueToLevelPointsButton();
    }
}
