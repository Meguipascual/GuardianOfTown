using UnityEngine;
using UnityEngine.UI;

public class FrontSwordPowerUp : PermanentPowerup
{
    private void Start()
    {
        _id = 1;
        base.Initialize();
        _thisButton = gameObject.GetComponent<Button>();
        if (_permanentPowerUpsSettings.IsFrontSwordActive)
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
        _permanentPowerUpsSettings.ActivateSword();
        _permanentPowerUpManager._notUsedPowerUpPrefabs.RemoveAt(Index);
        _sliderManager.ContinueToLevelPointsButton();
    }
}
