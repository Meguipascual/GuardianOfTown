using UnityEngine;
using UnityEngine.UI;

public class TownRecoveryPowerUp : PermanentPowerup
{
    private void Start()
    {
        _id = 0;
        base.Initialize();
        _thisButton = gameObject.GetComponent<Button>();
        if (GameManager.SharedInstance.TownHpShieldsDamaged > 0)
        {
            for (int i = 0; i < _permanentPowerUpsSettings.AreTownRecoveryWasted.Length; i++)
            {
                if (!_permanentPowerUpsSettings.AreTownRecoveryWasted[i])
                {
                    _thisButton.interactable = true;
                    return;
                }
            }
        }
        _thisButton.interactable = false;
    }
    public void Apply()
    {
        _permanentPowerUpsSettings.ActivateTownRecovery();
        _permanentPowerUpManager._notUsedPowerUpPrefabs.RemoveAt(Index);
        _sliderManager.ContinueToLevelPointsButton();
    }
}
